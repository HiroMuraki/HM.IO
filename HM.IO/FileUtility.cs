using System.Security.Cryptography;

namespace HM.IO;

public static class FileUtility
{
    #region ConstValues
    public static readonly Int32 LargeFileThreshold = 4096 * 1024;

    public static readonly Int32 LargeFileBufferSize = 64 * 1024;
    #endregion

    #region Methods
    public static async Task<Boolean> CompareEqualityAsync(EntryPath filePath1, EntryPath filePath2)
        => await CompareEqualityAsync(filePath1, filePath2, CancellationToken.None);

    public static async Task<Boolean> CompareEqualityAsync(EntryPath filePath1, EntryPath filePath2, CancellationToken cancellationToken)
    {
        if (!LocalFileIO.Exists(filePath1))
        {
            throw new FileNotFoundException($"{nameof(filePath1)} not found", filePath1.StringPath);
        }
        if (!LocalFileIO.Exists(filePath2))
        {
            throw new FileNotFoundException($"{nameof(filePath2)} not found", filePath2.StringPath);
        }

        if (filePath1 == filePath2)
        {
            return true;
        }

        try
        {
            using Stream fs1 = LocalFileIO.OpenRead(filePath1);
            using Stream fs2 = LocalFileIO.OpenRead(filePath2);
            if (fs1.Length != fs2.Length)
            {
                return false;
            }

            /* if the two files' size <= bufferThreshold, read the two to memory at once to compare 
             * or, read as blocks to compare */
            if (fs1.Length <= LargeFileThreshold)
            {
                Byte[] block1 = new Byte[fs1.Length];
                Byte[] block2 = new Byte[fs2.Length];
                fs1.Read(block1, 0, block1.Length);
                fs2.Read(block2, 0, block2.Length);

                if (block1.Length != block2.Length)
                {
                    return false;
                }

                for (Int32 i = 0; i < block1.Length; i++)
                {
                    if (block1[i] != block2[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                using var bf1 = new BufferedStream(fs1, LargeFileBufferSize);
                using var bf2 = new BufferedStream(fs2, LargeFileBufferSize);
                Byte[] block1 = new Byte[LargeFileBufferSize];
                Byte[] block2 = new Byte[LargeFileBufferSize];
                Int32 readCount1 = 0;
                Int32 readCount2 = 0;

                while (true)
                {
                    readCount1 = await bf1.ReadAsync(block1.AsMemory(0, LargeFileBufferSize), cancellationToken);
                    readCount2 = await bf2.ReadAsync(block2.AsMemory(0, LargeFileBufferSize), cancellationToken);

                    if (readCount1 != readCount2)
                    {
                        return false;
                    }

                    if (readCount1 == 0)
                    {
                        break;
                    }

                    for (Int32 i = 0; i < readCount1; i++)
                    {
                        if (block1[i] != block2[i])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    public static async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await CopyAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    public static async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    public static async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!LocalFileIO.Exists(sourceFilePath))
        {
            throw new FileNotFoundException($"Source file `{sourceFilePath.StringPath}` not found", sourceFilePath.StringPath);
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath.StringPath}` can be equal to `{sourceFilePath.StringPath}`.");
        }

        if (LocalFileIO.Exists(destinationFilePath))
        {
            Boolean fileEqual = await CompareEqualityAsync(sourceFilePath, destinationFilePath, cancellationToken);
            if (fileEqual)
            {
                return;
            }
            else if (overwrite)
            {
                LocalFileIO.Delete(destinationFilePath);
            }
            else
            {
                throw new IOException($"Destination file `{destinationFilePath}` already existed but not equal to `{sourceFilePath}`");
            }
        }

        using (Stream sourceFS = LocalFileIO.OpenRead(sourceFilePath))
        using (Stream destinationFS = LocalFileIO.OpenWrite(destinationFilePath))
        {
            await sourceFS.CopyToAsync(destinationFS, cancellationToken);
        }

        Boolean isOk = await CompareEqualityAsync(sourceFilePath, destinationFilePath, cancellationToken);
        if (!isOk)
        {
            throw new IOException($"Error on copying `{sourceFilePath}` to `{destinationFilePath}`");
        }
    }

    public static async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    public static async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await MoveAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    public static async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    public static async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!overwrite && LocalFileIO.Exists(destinationFilePath))
        {
            throw new ArgumentException($"Can't move `{sourceFilePath.StringPath}` to `{destinationFilePath.StringPath}` already existed.");
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath.StringPath}` can be equal to `{sourceFilePath.StringPath}`.");
        }

        EntryTimestamps sourceFileTimeStamps = LocalFileIO.GetFileTimestamps(sourceFilePath);
        FileAttributes fileAttributes = LocalFileIO.GetFileAttributes(sourceFilePath);

        if (Path.GetPathRoot(sourceFilePath.StringPath) == Path.GetPathRoot(destinationFilePath.StringPath))
        {
            if (LocalFileIO.Exists(destinationFilePath))
            {
                if (overwrite)
                {
                    LocalFileIO.Delete(destinationFilePath);
                }
                else
                {
                    throw new InvalidOperationException($"`Can't move `{sourceFilePath.StringPath}` to `{destinationFilePath.StringPath}` because `{destinationFilePath.StringPath}` already existed but `{nameof(overwrite)}` set to false.");
                }
            }

            LocalFileIO.Rename(sourceFilePath, destinationFilePath);
            LocalFileIO.SetFileTimestamps(destinationFilePath, sourceFileTimeStamps);
            LocalFileIO.SetFileAttributes(destinationFilePath, fileAttributes);
        }
        else
        {
            await CopyAsync(sourceFilePath, destinationFilePath, overwrite, cancellationToken);
            LocalFileIO.SetFileTimestamps(destinationFilePath, sourceFileTimeStamps);
            LocalFileIO.SetFileAttributes(destinationFilePath, fileAttributes);
            LocalFileIO.Delete(sourceFilePath);
        }
    }

    public static void CopyTimestamps(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        LocalFileIO.SetFileTimestamps(destinationFilePath, LocalFileIO.GetFileTimestamps(sourceFilePath));
    }

    public static void CopyAttributes(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        FileAttributes attributes = LocalFileIO.GetFileAttributes(sourceFilePath);
        LocalFileIO.SetFileAttributes(destinationFilePath, attributes);
    }

    public static async Task<String> ComputeHashAsync(EntryPath filePath)
        => await ComputeHashAsync(filePath, CancellationToken.None);

    public static async Task<String> ComputeHashAsync(EntryPath filePath, CancellationToken cancellationToken)
    {
        using var sha256 = SHA256.Create();
        using Stream reader = LocalFileIO.OpenRead(filePath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(reader, cancellationToken));
    }
    #endregion
}

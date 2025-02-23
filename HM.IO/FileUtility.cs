using HM.IO.Previews;

namespace HM.IO;

public static class FileUtility
{
    #region ConstValues
    public static readonly Int32 LargeFileThreshold = 4096 * 1024;

    public static readonly Int32 LargeFileBufferSize = 64 * 1024;
    #endregion

    #region Methods
    public static Boolean CompareEquality(String filePath1, String filePath2)
    {
        if (!File.Exists(filePath1))
        {
            throw new FileNotFoundException($"{nameof(filePath1)} not found", filePath1);
        }
        if (!File.Exists(filePath2))
        {
            throw new FileNotFoundException($"{nameof(filePath2)} not found", filePath2);
        }

        if (filePath1 == filePath2)
        {
            return true;
        }

        try
        {
            using Stream fs1 = File.OpenRead(filePath1);
            using Stream fs2 = File.OpenRead(filePath2);
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
                    readCount1 = bf1.Read(block1, 0, LargeFileBufferSize);
                    readCount2 = bf2.Read(block2, 0, LargeFileBufferSize);

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

    public static void Copy(String sourceFilePath, String destinationFilePath)
    {
        if (!File.Exists(sourceFilePath))
        {
            throw new FileNotFoundException($"Source file `{sourceFilePath}` not found", sourceFilePath);
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath}` can be equal to `{sourceFilePath}`.");
        }

        if (File.Exists(destinationFilePath))
        {
            Boolean fileEqual = CompareEquality(sourceFilePath, destinationFilePath);
            if (fileEqual)
            {
                return;
            }
            else
            {
                throw new IOException($"Destination file `{destinationFilePath}` already existed but not equal to `{sourceFilePath}`");
            }
        }

        using (Stream sourceFS = File.OpenRead(sourceFilePath))
        using (Stream destinationFS = File.OpenWrite(destinationFilePath))
        {
            sourceFS.CopyTo(destinationFS);
        }

        Boolean isOk = CompareEquality(sourceFilePath, destinationFilePath);
        if (!isOk)
        {
            throw new IOException($"Error on copying `{sourceFilePath}` to `{destinationFilePath}`");
        }
    }

    public static void Move(String sourceFilePath, String destinationFilePath)
    {
        if (File.Exists(destinationFilePath))
        {
            throw new ArgumentException($"Can't move `{sourceFilePath}` to `{destinationFilePath}` already existed.");
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath}` can be equal to `{sourceFilePath}`.");
        }

        EntryTimestamps sourceFileTimeStamps = LocalFileIO.GetFileTimestamps(new(sourceFilePath));
        FileAttributes fileAttributes = LocalFileIO.GetFileAttributes(new(sourceFilePath));

        if (Path.GetPathRoot(sourceFilePath) == Path.GetPathRoot(destinationFilePath))
        {
            if (File.Exists(destinationFilePath))
            {
                throw new InvalidOperationException($"`Can't move `{sourceFilePath}` to `{destinationFilePath}` because `{destinationFilePath}` already exists");
            }

            LocalFileIO.Rename(new(sourceFilePath), new(destinationFilePath));
            LocalFileIO.SetFileTimestamps(new(destinationFilePath), sourceFileTimeStamps);
            LocalFileIO.SetFileAttributes(new(destinationFilePath), fileAttributes);
        }
        else
        {
            Copy(sourceFilePath, new(destinationFilePath));
            LocalFileIO.SetFileTimestamps(new(destinationFilePath), sourceFileTimeStamps);
            LocalFileIO.SetFileAttributes(new(destinationFilePath), fileAttributes);
            LocalFileIO.Delete(new(sourceFilePath));
        }
    }

    public static void CopyTimestamps(String sourceFilePath, String destinationFilePath)
    {
        LocalFileIO.SetFileTimestamps(new(destinationFilePath), LocalFileIO.GetFileTimestamps(new(sourceFilePath)));
    }

    public static void CopyAttributes(String sourceFilePath, String destinationFilePath)
    {
        File.SetAttributes(destinationFilePath, File.GetAttributes(sourceFilePath));
    }
    #endregion
}

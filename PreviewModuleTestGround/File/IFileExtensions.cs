using HM.IO.Previews.File;
using HM.IO.Previews.Stream;

namespace HM.IO.Previews.File;

public static class IFileExtensions
{
    #region ConstValues
    public static readonly Int32 LargeFileThreshold = 4096 * 1024;

    public static readonly Int32 LargeFileBufferSize = 64 * 1024;
    #endregion

    #region Methods
    public static async Task CopyToAsync(this IFile self, IFile destinationFile, Boolean overwrite, CancellationToken cancellationToken)
    {
        IFile sourceFile = self;

        if (!sourceFile.Exists)
        {
            throw new FileNotFoundException($"Source file `{sourceFile.Path.StringPath}` not found", sourceFile.Path.StringPath);
        }

        if (sourceFile.Path == destinationFile.Path)
        {
            throw new InvalidOperationException($"The path of source file `{destinationFile.Path.StringPath}` can not equal to the path of destination path`{sourceFile.Path.StringPath}`");
        }

        if (destinationFile.Exists)
        {
            Boolean fileEqual = await sourceFile.CompareEqualityAsync(destinationFile, cancellationToken);
            if (fileEqual)
            {
                return;
            }
            else if (overwrite)
            {
                destinationFile.Delete();
            }
            else
            {
                throw new IOException($"Destination file `{destinationFile.Path.StringPath}` already existed but not equal to `{sourceFile.Path.StringPath}`");
            }
        }

        using (IStream sourceFileStream = sourceFile.Open(StreamMode.ReadOnly))
        using (IStream destinationFileStream = destinationFile.Open(StreamMode.WriteOnly))
        {
            await sourceFileStream.CopyToAsync(destinationFileStream, cancellationToken);
        }

        Boolean isOk = await sourceFile.CompareEqualityAsync(destinationFile, cancellationToken);
        if (!isOk)
        {
            throw new IOException($"Error on copying `{sourceFile.Path.StringPath}` to `{destinationFile.Path.StringPath}`");
        }
    }

    public static async Task MoveToAsync(this IFile self, IFile destinationFile, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!overwrite && destinationFile.Exists)
        {
            throw new ArgumentException($"`{self.Path.StringPath}` to `{destinationFile.Path.StringPath}` already existed");
        }

        if (self.Path == destinationFile.Path)
        {
            throw new InvalidOperationException($"`{destinationFile.Path.StringPath}` can't equal to `{self.Path.StringPath}`");
        }

        EntryTimestamps sourceFileTimeStamps = self.Timestamps;
        EntryAttributes sourceFileAttributes = self.Attributes;

        if (Path.GetPathRoot(self.Path.StringPath) == Path.GetPathRoot(destinationFile.Path.StringPath))
        {
            if (destinationFile.Exists)
            {
                if (overwrite)
                {
                    destinationFile.Delete();
                }
                else
                {
                    throw new InvalidOperationException($"`{destinationFile.Path.StringPath}` already existed but `{nameof(overwrite)}` set to false");
                }
            }

            throw new NotSupportedException();
        }
        else
        {
            await self.CopyToAsync(destinationFile, overwrite, cancellationToken);
            self.Delete();
        }

        destinationFile.Timestamps = sourceFileTimeStamps;
        destinationFile.Attributes = sourceFileAttributes;
    }

    public static async Task<Boolean> CompareEqualityAsync(this IFile self, IFile other, CancellationToken cancellationToken)
    {
        if (!self.Exists)
        {
            throw new FileNotFoundException(String.Empty, self.Path.StringPath);
        }
        if (!other.Exists)
        {
            throw new FileNotFoundException(String.Empty, other.Path.StringPath);
        }

        if (self.Path == other.Path)
        {
            return true;
        }

        try
        {
            using IStream fs1 = self.Open(StreamMode.ReadOnly);
            using IStream fs2 = other.Open(StreamMode.ReadOnly);
            if (fs1.SizeInBytes != fs2.SizeInBytes)
            {
                return false;
            }

            /* if the two files' size <= bufferThreshold, read the two to memory at once to compare 
             * or, read as blocks to compare */
            if (fs1.SizeInBytes <= LargeFileThreshold)
            {
                Byte[] block1 = new Byte[fs1.SizeInBytes];
                Byte[] block2 = new Byte[fs2.SizeInBytes];
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
                using var bf1 = new BufferedStream(fs1.GetBclStream(), LargeFileBufferSize);
                using var bf2 = new BufferedStream(fs2.GetBclStream(), LargeFileBufferSize);
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
    #endregion
}

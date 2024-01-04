using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace HM.IO;

/// <include file='Docs/FileUtility.xml' path='FileUtility/Class[@name="FileUtility"]/*' />
public class FileUtility
{
    #region ConstValues
    /// <include file='Docs/FileUtility.xml' path='FileUtility/ConstValues/Static[@name="LargeFileThreshold"]/*' />
    public const Int32 LargeFileThreshold = 4096 * 1024;

    /// <include file='Docs/FileUtility.xml' path='FileUtility/ConstValues/Static[@name="LargeFileBufferSize"]/*' />
    public const Int32 LargeFileBufferSize = 64 * 1024;
    #endregion

    #region Properties
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Properties/Static[@name="Default"]/*' />
    public static FileUtility Default { get; } = new(new FileIO());
    #endregion

    #region Methods
    #region CompareEquality
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CompareEqualityAsync[EntryPath,EntryPath]"]/*' />
    public async Task<Boolean> CompareEqualityAsync(EntryPath filePath1, EntryPath filePath2)
        => await CompareEqualityAsync(filePath1, filePath2, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CompareEqualityAsync[EntryPath,EntryPath,CancellationToken]"]/*' />
    public async Task<Boolean> CompareEqualityAsync(EntryPath filePath1, EntryPath filePath2, CancellationToken cancellationToken)
    {
        if (!_fileIO.Exists(filePath1))
        {
            throw new FileNotFoundException($"{nameof(filePath1)} not found", filePath1.StringPath);
        }
        if (!_fileIO.Exists(filePath2))
        {
            throw new FileNotFoundException($"{nameof(filePath2)} not found", filePath2.StringPath);
        }

        if (filePath1 == filePath2)
        {
            return true;
        }

        try
        {
            using Stream fs1 = _fileIO.OpenRead(filePath1);
            using Stream fs2 = _fileIO.OpenRead(filePath2);
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
    #endregion

    #region Copy
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyAsync[EntryPath,EntryPath]"]/*' />
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyAsync[EntryPath,EntryPath,Boolean]"]/*' />
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await CopyAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyAsync[EntryPath,EntryPath,CancellationToken]"]/*' />
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyAsync[EntryPath,EntryPath,Boolean,CancellationToken]"]/*' />
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!_fileIO.Exists(sourceFilePath))
        {
            throw new FileNotFoundException($"Source file `{sourceFilePath.StringPath}` not found", sourceFilePath.StringPath);
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath.StringPath}` can be equal to `{sourceFilePath.StringPath}`.");
        }

        if (_fileIO.Exists(destinationFilePath))
        {
            Boolean fileEqual = await CompareEqualityAsync(sourceFilePath, destinationFilePath, cancellationToken);
            if (fileEqual)
            {
                return;
            }
            else if (overwrite)
            {
                _fileIO.Delete(destinationFilePath);
            }
            else
            {
                throw new IOException($"Destination file `{destinationFilePath}` already existed but not equal to `{sourceFilePath}`");
            }
        }

        using (Stream sourceFS = _fileIO.OpenRead(sourceFilePath))
        using (Stream destinationFS = _fileIO.OpenWrite(destinationFilePath))
        {
            await sourceFS.CopyToAsync(destinationFS, cancellationToken);
        }

        Boolean isOk = await CompareEqualityAsync(sourceFilePath, destinationFilePath, cancellationToken);
        if (!isOk)
        {
            throw new IOException($"Error on copying `{sourceFilePath}` to `{destinationFilePath}`");
        }
    }
    #endregion

    #region Move
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="MoveAsync[EntryPath,EntryPath]"]/*' />
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="MoveAsync[EntryPath,EntryPath,Boolean]"]/*' />
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await MoveAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="MoveAsync[EntryPath,EntryPath,CancellationToken]"]/*' />
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="MoveAsync[EntryPath,EntryPath,Boolean,CancellationToken]"]/*' />
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!overwrite && _fileIO.Exists(destinationFilePath))
        {
            throw new ArgumentException($"Can't move `{sourceFilePath.StringPath}` to `{destinationFilePath.StringPath}` already existed.");
        }

        if (sourceFilePath == destinationFilePath)
        {
            throw new InvalidOperationException($"The `{destinationFilePath.StringPath}` can be equal to `{sourceFilePath.StringPath}`.");
        }

        FileTimestamps sourceFileTimeStamps = _fileIO.GetFileTimestamps(sourceFilePath);
        FileAttributes fileAttributes = _fileIO.GetFileAttributes(sourceFilePath);

        if (Path.GetPathRoot(sourceFilePath.StringPath) == Path.GetPathRoot(destinationFilePath.StringPath))
        {
            if (_fileIO.Exists(destinationFilePath))
            {
                if (overwrite)
                {
                    _fileIO.Delete(destinationFilePath);
                }
                else
                {
                    throw new InvalidOperationException($"`Can't move `{sourceFilePath.StringPath}` to `{destinationFilePath.StringPath}` because `{destinationFilePath.StringPath}` already existed but `{nameof(overwrite)}` set to false.");
                }
            }

            _fileIO.Rename(sourceFilePath, destinationFilePath);
            _fileIO.SetFileTimestamps(destinationFilePath, sourceFileTimeStamps);
            _fileIO.SetFileAttributes(destinationFilePath, fileAttributes);
        }
        else
        {
            await CopyAsync(sourceFilePath, destinationFilePath, overwrite, cancellationToken);
            _fileIO.SetFileTimestamps(destinationFilePath, sourceFileTimeStamps);
            _fileIO.SetFileAttributes(destinationFilePath, fileAttributes);
            _fileIO.Delete(sourceFilePath);
        }
    }
    #endregion

    #region MetaData
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyTimestamps[EntryPath,EntryPath]"]/*' />
    public void CopyTimestamps(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        _fileIO.SetFileTimestamps(destinationFilePath, _fileIO.GetFileTimestamps(sourceFilePath));
    }

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="CopyAttributes[EntryPath,EntryPath]"]/*' />
    public void CopyAttributes(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        FileAttributes attributes = _fileIO.GetFileAttributes(sourceFilePath);
        _fileIO.SetFileAttributes(destinationFilePath, attributes);
    }
    #endregion

    #region ComputeHash
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="ComputeHashAsync[EntryPath]"]/*' />
    public async Task<String> ComputeHashAsync(EntryPath filePath)
        => await ComputeHashAsync(filePath, CancellationToken.None);

    /// <include file='Docs/FileUtility.xml' path='FileUtility/Methods/Instance[@name="ComputeHashAsync[EntryPath,CancellationToken]"]/*' />
    public async Task<String> ComputeHashAsync(EntryPath filePath, CancellationToken cancellationToken)
    {
        using var sha256 = SHA256.Create();
        using Stream reader = _fileIO.OpenRead(filePath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(reader, cancellationToken));
    }
    #endregion
    #endregion

    #region Constructors
    /// <include file='Docs/FileUtility.xml' path='FileUtility/Ctors/Ctor[@name="FileUtility[IFileIO]"]/*' />
    public FileUtility(IFileIO fileIO)
    {
        _fileIO = fileIO;
    }
    #endregion

    #region NonPublic
    private readonly IFileIO _fileIO;
    #endregion
}

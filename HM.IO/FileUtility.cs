using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace HM.IO;

/// <summary>
/// A utility class for file-related operations. This is not a static class for providing a more flexible class.
/// </summary>
public class FileUtility
{
    /// <summary>
    /// The threshold size in bytes for a file to be considered large.
    /// </summary>
    public const Int32 LargeFileThreshold = 4096 * 1024;
    /// <summary>
    /// The buffer size in bytes to use when reading and writing large files.
    /// </summary>
    public const Int32 LargeFileBufferSize = 64 * 1024;

    public static FileUtility Default { get; } = new(new FileIO());

    #region Methods
    #region CompareEquality
    /// <summary>
    /// Compares the contents of two files for equality.
    /// </summary>
    /// <param name="filePath1">The first file to compare.</param>
    /// <param name="filePath2">The second file to compare.</param>
    /// <returns>true if the files have the same contents; otherwise, false.</returns>
    public async Task<Boolean> CompareEqualityAsync(EntryPath filePath1, EntryPath filePath2)
        => await CompareEqualityAsync(filePath1, filePath2, CancellationToken.None);

    /// <summary>
    /// Compares the contents of two files for equality, with support for cancellation.
    /// </summary>
    /// <param name="file1">The first file to compare.</param>
    /// <param name="file2">The second file to compare.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <returns>true if the files have the same contents; otherwise, false.</returns>
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
    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await CopyAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task CopyAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
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
    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await MoveAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public async Task MoveAsync(EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
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
    /// <summary>
    /// Copies the timestamps (creation time, last write time, and last access time) from the source file to the destination file.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    public void CopyTimestamps(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        _fileIO.SetFileTimestamps(destinationFilePath, _fileIO.GetFileTimestamps(sourceFilePath));
    }

    /// <summary>
    /// Copies the file attributes (such as read-only or hidden) from the source file to the destination file.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    public void CopyAttributes(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        FileAttributes attributes = _fileIO.GetFileAttributes(sourceFilePath);
        _fileIO.SetFileAttributes(destinationFilePath, attributes);
    }
    #endregion

    #region ComputeHashAsync
    /// <summary>
    /// Asynchronously computes the hash value of a file using a hashing algorithm.
    /// </summary>
    /// <param name="filePath">The path to the file for which to compute the hash.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a string
    /// containing the computed hash value of the file, represented as a hexadecimal string.
    /// </returns>
    public async Task<String> ComputeHashAsync(EntryPath filePath)
        => await ComputeHashAsync(filePath, CancellationToken.None);

    /// <summary>
    /// Asynchronously computes the hash value of a file using a hashing algorithm.
    /// </summary>
    /// <param name="filePath">The path to the file for which to compute the hash.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to request the operation's cancellation.
    /// This allows the caller to request the task to be canceled if needed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a string
    /// containing the computed hash value of the file, represented as a hexadecimal string.
    /// </returns>
    public async Task<String> ComputeHashAsync(EntryPath filePath, CancellationToken cancellationToken)
    {
        using var sha256 = SHA256.Create();
        using Stream reader = _fileIO.OpenRead(filePath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(reader, cancellationToken));
    }
    #endregion
    #endregion

    public FileUtility(IFileIO fileIO)
    {
        _fileIO = fileIO;
    }

    #region NonPublic
    private readonly IFileIO _fileIO;
    #endregion
}
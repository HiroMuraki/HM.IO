using System.Security.Cryptography;

namespace HM.IO;

/// <summary>
/// A utility class for file-related operations.
/// </summary>
public static class FileUtil
{
    /// <summary>
    /// The threshold size in bytes for a file to be considered large.
    /// </summary>
    public const Int32 LargeFileThreshold = 4096 * 1024;
    /// <summary>
    /// The buffer size in bytes to use when reading and writing large files.
    /// </summary>
    public const Int32 LargeFileBufferSize = 64 * 1024;

    #region CompareEqualityAsync
    /// <summary>
    /// Compares the contents of two files for equality.
    /// </summary>
    /// <param name="file1">The first file to compare.</param>
    /// <param name="file2">The second file to compare.</param>
    /// <returns>true if the files have the same contents; otherwise, false.</returns>
    public static async Task<Boolean> CompareEqualityAsync(String file1, String file2)
        => await CompareEqualityAsync(file1, file2, CancellationToken.None);

    /// <summary>
    /// Compares the contents of two files for equality, with support for cancellation.
    /// </summary>
    /// <param name="file1">The first file to compare.</param>
    /// <param name="file2">The second file to compare.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <returns>true if the files have the same contents; otherwise, false.</returns>
    public static async Task<Boolean> CompareEqualityAsync(String file1, String file2, CancellationToken cancellationToken)
    {
        if (file1 == file2)
        {
            return true;
        }

        try
        {
            using FileStream fs1 = File.OpenRead(file1);
            using FileStream fs2 = File.OpenRead(file2);
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

    #region CopyAsync
    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public static async Task CopyAsync(String sourceFilePath, String destinationFilePath)
        => await CopyAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <summary>
    /// Copies a file from the source path to the destination path, with options to overwrite the destination file and support for cancellation.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public static async Task CopyAsync(String sourceFilePath, String destinationFilePath, Boolean overwrite)
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
    public static async Task CopyAsync(String sourceFilePath, String destinationFilePath, CancellationToken cancellationToken)
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
    public static async Task CopyAsync(String sourceFilePath, String destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!File.Exists(sourceFilePath))
        {
            throw new FileNotFoundException($"Source file `{sourceFilePath}` not found", sourceFilePath);
        }

        if (File.Exists(destinationFilePath))
        {
            Boolean fileEqual = await CompareEqualityAsync(sourceFilePath, destinationFilePath, cancellationToken);
            if (fileEqual)
            {
                return;
            }
            else if (overwrite)
            {
                File.Delete(destinationFilePath);
            }
            else
            {
                throw new IOException($"Destination file `{destinationFilePath}` already existed but not equal to `{sourceFilePath}`");
            }
        }

        using (FileStream sourceFS = File.OpenRead(sourceFilePath))
        using (FileStream destinationFS = File.OpenWrite(destinationFilePath))
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

    #region MoveAsync
    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public static async Task MoveAsync(String sourceFilePath, String destinationFilePath)
        => await MoveAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="overwrite">Whether to overwrite the destination file if it already exists.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public static async Task MoveAsync(String sourceFilePath, String destinationFilePath, Boolean overwrite)
        => await MoveAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    /// <summary>
    /// Moves a file from the source path to the destination path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the operation.</param>
    /// <exception cref="FileNotFoundException">Thrown if the source file does not exist.</exception>
    /// <exception cref="IOException">Thrown if the destination file exists and overwrite is false, or if the copied file's contents do not match the original file's contents.</exception>
    public static async Task MoveAsync(String sourceFilePath, String destinationFilePath, CancellationToken cancellationToken)
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
    public static async Task MoveAsync(String sourceFilePath, String destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
    {
        if (!overwrite && File.Exists(destinationFilePath))
        {
            throw new ArgumentException($"Can't move `{sourceFilePath}` to `{destinationFilePath}` already existed.");
        }

        DateTime creationTime = File.GetCreationTime(sourceFilePath);
        DateTime lastWriteTime = File.GetLastWriteTime(sourceFilePath);
        DateTime lastAccessTime = File.GetLastAccessTime(sourceFilePath);
        FileAttributes fileAttributes = File.GetAttributes(sourceFilePath);

        if (Path.GetPathRoot(sourceFilePath) == Path.GetPathRoot(destinationFilePath))
        {
            File.Move(sourceFilePath, destinationFilePath, overwrite);
        }
        else
        {
            await CopyAsync(sourceFilePath, destinationFilePath, overwrite, cancellationToken);
            File.Delete(sourceFilePath);
        }

        File.SetCreationTime(destinationFilePath, creationTime);
        File.SetLastWriteTime(destinationFilePath, lastWriteTime);
        File.SetLastAccessTime(destinationFilePath, lastAccessTime);
        File.SetAttributes(destinationFilePath, fileAttributes);
    }
    #endregion

    #region Rename
    public static void Rename(String filePath, String newName)
    {
        String? directoryName = Path.GetDirectoryName(filePath);
        String name = Path.Combine(directoryName ?? String.Empty, newName);
        File.Move(filePath, name);
    }
    #endregion

    #region MetaData
    /// <summary>
    /// Copies the timestamps (creation time, last write time, and last access time) from the source file to the destination file.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    public static void CopyTimestamps(String sourceFilePath, String destinationFilePath)
    {
        FileAttributes attributes = File.GetAttributes(sourceFilePath);
        File.SetAttributes(destinationFilePath, attributes);

        File.SetCreationTimeUtc(destinationFilePath, File.GetCreationTimeUtc(sourceFilePath));
        File.SetLastAccessTimeUtc(destinationFilePath, File.GetLastAccessTimeUtc(sourceFilePath));
        File.SetLastWriteTimeUtc(destinationFilePath, File.GetLastWriteTimeUtc(sourceFilePath));
    }

    /// <summary>
    /// Copies the file attributes (such as read-only or hidden) from the source file to the destination file.
    /// </summary>
    /// <param name="sourceFilePath">The path of the source file.</param>
    /// <param name="destinationFilePath">The path of the destination file.</param>
    public static void CopyAttributes(String sourceFilePath, String destinationFilePath)
    {
        File.SetAttributes(destinationFilePath, File.GetAttributes(sourceFilePath));
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
    public static async Task<String> ComputeHashAsync(String filePath)
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
    public static async Task<String> ComputeHashAsync(String filePath, CancellationToken cancellationToken)
    {
        using var sha256 = SHA256.Create();
        using FileStream reader = File.OpenRead(filePath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(reader, cancellationToken));
    }
    #endregion
}
namespace HM.IO;

/// <summary>
/// 	Default implementation for <see cref="IFileIO"/>.
/// </summary>
public class FileIO : IFileIO
{
    /// <summary>
    /// 	Instance of <see cref="FileIO"/>.
    /// </summary>
    /// <value>
    /// 	A <see cref="FileIO"/> instance.
    /// </value>
    public static FileIO Default { get; } = new();

    /// <summary>
    /// 	Checks if the specified file exists at the given file path.
    /// </summary>
    /// <param name="filePath">The path of the file to check.</param>
    /// <returns>
    /// 	True if the file exists; otherwise, false.
    /// </returns>
    public Boolean Exists(EntryPath filePath)
    {
        return File.Exists(filePath.StringPath);
    }

    /// <summary>
    /// 	Renames the file specified by the source file path to the destination file path.
    /// </summary>
    /// <param name="sourceFilePath">The path of the file to rename.</param>
    /// <param name="destinationFilePath">The new path to which the file should be renamed.</param>
    /// <returns>
    /// 	True if the renaming was successful; otherwise, false.
    /// </returns>
    public void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        File.Move(sourceFilePath.StringPath, destinationFilePath.StringPath);
    }

    /// <summary>
    /// 	Deletes the file at the specified file path.
    /// </summary>
    /// <param name="filePath">The path of the file to delete.</param>
    /// <returns>
    /// 	True if the file was successfully deleted; otherwise, false.
    /// </returns>
    public void Delete(EntryPath filePath)
    {
        File.Delete(filePath.StringPath);
    }

    /// <summary>
    /// 	Opens the specified file for reading.
    /// </summary>
    /// <param name="filePath">The path of the file to open for reading.</param>
    /// <returns>
    /// 	A <see cref="Stream"/> representing the contents of the file.
    /// </returns>
    public Stream OpenRead(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }

    /// <summary>
    /// 	Opens the specified file for writing, creating the file if it does not exist.
    /// </summary>
    /// <param name="filePath">The path of the file to open for writing.</param>
    /// <returns>
    /// 	A <see cref="Stream"/> for writing to the file.
    /// </returns>
    public Stream OpenWrite(EntryPath filePath)
    {
        return File.OpenWrite(filePath.StringPath);
    }

    /// <summary>
    /// 	Gets the creation, last access, and last write timestamps of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to get timestamps for.</param>
    /// <returns>
    /// 	A <see cref="FileTimestamps"/> object containing the timestamps.
    /// </returns>
    public EntryTimestamps GetFileTimestamps(EntryPath path)
    {
        String filePath = path.StringPath;

        return new EntryTimestamps
        {
            CreationTime = File.GetCreationTime(filePath),
            LastWriteTime = File.GetLastWriteTime(filePath),
            LastAccessTime = File.GetLastAccessTime(filePath),
        };
    }

    /// <summary>
    /// 	Sets the creation, last access, and last write timestamps of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to set timestamps for.</param>
    /// <param name="timestamps">
    /// 	A <see cref="FileTimestamps"/> object containing the timestamps to set.
    /// </param>
    /// <returns>
    /// 	True if the timestamps were successfully set; otherwise, false.
    /// </returns>
    public void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps)
    {
        String filePath = path.StringPath;

        File.SetCreationTime(filePath, timestamps.CreationTime);
        File.SetLastWriteTime(filePath, timestamps.LastWriteTime);
        File.SetLastAccessTime(filePath, timestamps.LastAccessTime);
    }

    /// <summary>
    /// 	Gets the attributes of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to get attributes for.</param>
    /// <returns>
    /// 	A <see cref="FileAttributes"/> object representing the attributes of the file.
    /// </returns>
    public FileAttributes GetFileAttributes(EntryPath path)
    {
        return File.GetAttributes(path.StringPath);
    }

    /// <summary>
    /// 	Sets the attributes of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to set attributes for.</param>
    /// <param name="attributes">
    /// 	A <see cref="FileAttributes"/> object representing the attributes to set.
    /// </param>
    /// <returns>
    /// 	True if the attributes were successfully set; otherwise, false.
    /// </returns>
    public void SetFileAttributes(EntryPath path, FileAttributes attributes)
    {
        File.SetAttributes(path.StringPath, attributes);
    }

    #region NonPublic
    private FileIO()
    {

    }
    #endregion
}

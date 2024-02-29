namespace HM.IO;

/// <summary>
/// 	Interface defining a contract for basic file operations.
/// </summary>
public interface IFileIO
{
    /// <summary>
    /// 	Checks if the specified file exists at the given file path.
    /// </summary>
    /// <param name="path">The path of the file to check for existence.</param>
    /// <returns>
    ///     True if the file exists; otherwise, false.
    /// </returns>
    Boolean Exists(EntryPath path);

    /// <summary>
    /// 	Opens the specified file for reading.
    /// </summary>
    /// <param name="path">The path of the file to open for reading.</param>
    /// <returns>
    ///     A stream for reading the contents of the file.
    /// </returns>
    Stream OpenRead(EntryPath path);

    /// <summary>
    /// 	Opens the specified file for writing, creating the file if it does not exist.
    /// </summary>
    /// <param name="path">The path of the file to open for writing.</param>
    /// <returns>
    ///     A stream for writing to the file.
    /// </returns>
    Stream OpenWrite(EntryPath path);

    /// <summary>
    /// 	Renames the file specified by the source file path to the destination file path.
    /// </summary>
    /// <param name="sourceFilePath">The current path of the file to be renamed.</param>
    /// <param name="destinationFilePath">The new path to which the file should be renamed.</param>
    /// <returns>
    ///     True if the renaming was successful; otherwise, false.
    /// </returns>
    void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath);

    /// <summary>
    /// 	Deletes the file at the specified file path.
    /// </summary>
    /// <param name="path">The path of the file to delete.</param>
    /// <returns>
    ///     True if the file was successfully deleted; otherwise, false.
    /// </returns>
    void Delete(EntryPath path);

    /// <summary>
    /// 	Gets the creation, last access, and last write timestamps of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to get timestamps for.</param>
    /// <returns>
    /// 	A <see cref="FileTimestamps"/> object containing the timestamps.
    /// </returns>
    EntryTimestamps GetFileTimestamps(EntryPath path);

    /// <summary>
    /// 	Sets the creation, last access, and last write timestamps of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to set timestamps for.</param>
    /// <param name="timestamps">A <see cref="FileTimestamps"/> object containing the timestamps to set.</param>
    /// <returns>
    ///     True if the timestamps were successfully set; otherwise, false.
    /// </returns>
    void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps);

    /// <summary>
    /// 	Gets the attributes of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to get attributes for.</param>
    /// <returns>
    /// 	A <see cref="FileAttributes"/> object representing the attributes of the file.
    /// </returns>
    FileAttributes GetFileAttributes(EntryPath path);

    /// <summary>
    /// 	Sets the attributes of the specified file.
    /// </summary>
    /// <param name="path">The path of the file to set attributes for.</param>
    /// <param name="attributes">A <see cref="FileAttributes"/> object representing the attributes to set.</param>
    /// <returns>
    ///     True if the attributes were successfully set; otherwise, false.
    /// </returns>
    void SetFileAttributes(EntryPath path, FileAttributes attributes);
}

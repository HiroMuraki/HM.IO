namespace HM.IO;

/// <summary>
/// 	Represents an interface for performing directory-related I/O operations.
/// </summary>
public interface IDirectoryIO
{
    EntryTimestamps GetFileTimestamps(EntryPath path);

    /// <summary>
    /// 	Checks if the specified directory or file exists at the given entry path.
    /// </summary>
    /// <param name="entryPath">The path of the directory or file to check for existence.</param>
    /// <returns>
    /// 	True if the directory or file exists; otherwise, false.
    /// </returns>
    Boolean Exists(EntryPath path);

    /// <summary>
    /// 	Enumerates directories in the specified path with the given options.
    /// </summary>
    /// <param name="path">The path to enumerate directories from.</param>
    /// <param name="enumerationOptions">Options to control the directory enumeration.</param>
    /// <returns>
    /// 	An enumerable collection of directory <see cref="EntryPath"/>.
    /// </returns>
    IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions);

    /// <summary>
    /// 	Enumerates files in the specified path with the given options.
    /// </summary>
    /// <param name="path">The path to enumerate files from.</param>
    /// <param name="enumerationOptions">Options to control the file enumeration.</param>
    /// <returns>
    /// 	An enumerable collection of file <see cref="EntryPath"/>.
    /// </returns>
    IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions);
}

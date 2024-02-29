namespace HM.IO;

/// <summary>
/// 	Default implementation for <see cref="IDirectoryIO"/>.
/// </summary>
public sealed class DirectoryIO :
    IDirectoryIO
{
    /// <summary>
    /// 	Instance of <see cref="DirectoryIO"/>.
    /// </summary>
    /// <value>
    /// 	A <see cref="DirectoryIO"/> instance.
    /// </value>
    public static DirectoryIO Default { get; } = new();

    /// <summary>
    /// 	Checks if the specified directory or file exists at the given entry path.
    /// </summary>
    /// <param name="entryPath">The path of the directory or file to check for existence.</param>
    /// <returns>
    /// 	True if the directory or file exists; otherwise, false.
    /// </returns>
    public EntryTimestamps GetFileTimestamps(EntryPath path)
    {
        var stringPath = path.StringPath;

        return new EntryTimestamps
        {
            CreationTime = File.GetCreationTime(stringPath),
            LastWriteTime = File.GetCreationTime(stringPath),
            LastAccessTime = File.GetCreationTime(stringPath),
        };
    }

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    public Boolean Exists(EntryPath entryPath)
    {
        return Directory.Exists(entryPath.StringPath);
    }

    /// <summary>
    /// 	Returns an enumerable collection of directory names in the specified directory and matching the specified search pattern and options.
    /// </summary>
    /// <param name="path">The path to the directory to enumerate.</param>
    /// <param name="enumerationOptions">An object that specifies options to control the search behavior, such as search pattern and recursion depth.</param>
    /// <returns>
    /// 	An enumerable collection of directory names that match the specified criteria.
    /// </returns>
    public IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.Create);
    }

    /// <summary>
    /// 	Returns an enumerable collection of file names in the specified directory and matching the specified search pattern and options.
    /// </summary>
    /// <param name="path">The path to the directory to enumerate.</param>
    /// <param name="enumerationOptions">An object that specifies options to control the search behavior, such as search pattern and recursion depth.</param>
    /// <returns>
    /// 	An enumerable collection of file names that match the specified criteria.
    /// </returns>
    public IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateFiles(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.Create);
    }

    #region NonPublic
    private DirectoryIO()
    {

    }
    #endregion
}
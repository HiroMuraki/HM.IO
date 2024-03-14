namespace HM.IO;

public sealed class LocalDirectoryIO :
    IDirectoryIO
{
    public static LocalDirectoryIO Default { get; } = new();

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

    public Boolean Exists(EntryPath entryPath)
    {
        return Directory.Exists(entryPath.StringPath);
    }

    public IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.Create);
    }

    public IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateFiles(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.Create);
    }

    #region NonPublic
    private LocalDirectoryIO()
    {

    }
    #endregion
}
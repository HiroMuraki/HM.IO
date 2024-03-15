namespace HM.IO;

public sealed class LocalDirectoryIO
{
    public static Boolean Exists(EntryPath entryPath)
    {
        return Directory.Exists(entryPath.StringPath);
    }

    public static IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(x => new EntryPath(x));
    }

    public static IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateFiles(path.StringPath, "*", enumerationOptions)
            .Select(x => new EntryPath(x));
    }

    #region NonPublic
    private LocalDirectoryIO()
    {

    }
    #endregion
}
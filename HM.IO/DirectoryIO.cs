namespace HM.IO;

/// <summary>
/// Default implementation for <see cref="IDirectoryIO"/>
/// </summary>
public sealed class DirectoryIO :
    IDirectoryIO
{
    public Boolean Exists(EntryPath entryPath)
    {
        return Directory.Exists(entryPath.StringPath);
    }

    public IEnumerable<EntryPath> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.CreateFromPath);
    }

    public IEnumerable<EntryPath> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateFiles(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.CreateFromPath);
    }
}
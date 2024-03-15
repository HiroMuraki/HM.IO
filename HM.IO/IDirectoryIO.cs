namespace HM.IO;

public interface IDirectoryIO
{
    static abstract Boolean Exists(EntryPath path);

    static abstract IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions);

    static abstract IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions);
}

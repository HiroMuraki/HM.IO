namespace HM.IO;

public interface IDirectoryIO
{
    EntryTimestamps GetFileTimestamps(EntryPath path);

    Boolean Exists(EntryPath path);

    IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions);

    IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions);
}

namespace HM.IO;

public sealed class DirectoryIO : IDirectoryIO
{
    public IEnumerable<String> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory.EnumerateDirectories(path.StringPath, "*", enumerationOptions);
    }

    public IEnumerable<String> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory.EnumerateFiles(path.StringPath, "*", enumerationOptions);
    }
}

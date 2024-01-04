namespace HM.IO;

/// <include file='DirectoryIO.xml' path='DirectoryIO/Class[@name="DirectoryIO"]/*' />
public sealed class DirectoryIO :
    IDirectoryIO
{
    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    public Boolean Exists(EntryPath entryPath)
    {
        return Directory.Exists(entryPath.StringPath);
    }

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="EnumerateDirectories[EntryPath,EnumerationOptions]"]/*' />
    public IEnumerable<EntryPath> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.CreateFromPath);
    }

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="EnumerateFiles[EntryPath,EnumerationOptions]"]/*' />
    public IEnumerable<EntryPath> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateFiles(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.CreateFromPath);
    }
}
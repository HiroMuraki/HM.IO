namespace HM.IO;

/// <include file='DirectoryIO.xml' path='DirectoryIO/Class[@name="DirectoryIO"]/*' />
public sealed class DirectoryIO :
    IDirectoryIO
{
    /// <include file='DirectoryIO.xml' path='DirectoryIO/Properties/Static[@name="Default"]/*' />
    public static DirectoryIO Default { get; } = new();

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="GetFileTimestamps[EntryPath]"]/*' />
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

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="EnumerateDirectories[EntryPath,EnumerationOptions]"]/*' />
    public IEnumerable<EntryPath> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions)
    {
        return Directory
            .EnumerateDirectories(path.StringPath, "*", enumerationOptions)
            .Select(EntryPath.Create);
    }

    /// <include file='DirectoryIO.xml' path='DirectoryIO/Methods/Instance[@name="EnumerateFiles[EntryPath,EnumerationOptions]"]/*' />
    public IEnumerable<EntryPath> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions)
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
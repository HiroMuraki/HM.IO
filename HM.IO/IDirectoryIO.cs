namespace HM.IO;

/// <include file='IDirectoryIO.xml' path='IDirectoryIO/Class[@name="IDirectoryIO"]/*' />
public interface IDirectoryIO
{
    EntryTimestamps GetFileTimestamps(EntryPath path);

    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    Boolean Exists(EntryPath path);

    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="EnumerateFilePaths[EntryPath,EnumerationOptions]"]/*' />
    IEnumerable<EntryPath> EnumerateFilePaths(EntryPath path, EnumerationOptions enumerationOptions);

    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="EnumerateDirectoryPaths[EntryPath,EnumerationOptions]"]/*' />
    IEnumerable<EntryPath> EnumerateDirectoryPaths(EntryPath path, EnumerationOptions enumerationOptions);
}

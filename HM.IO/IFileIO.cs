namespace HM.IO;

/// <include file='Docs/IFileIO.xml' path='IFileIO/Class[@name="IFileIO"]/*' />
public interface IFileIO
{
    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    Boolean Exists(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="OpenRead[EntryPath]"]/*' />
    Stream OpenRead(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="OpenWrite[EntryPath]"]/*' />
    Stream OpenWrite(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="Rename[EntryPath,EntryPath]"]/*' />
    void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="Delete[EntryPath]"]/*' />
    void Delete(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="GetFileTimestamps[EntryPath]"]/*' />
    FileTimestamps GetFileTimestamps(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="SetFileTimestamps[EntryPath,FileTimestamps]"]/*' />
    void SetFileTimestamps(EntryPath path, FileTimestamps timestamps);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="GetFileAttributes[EntryPath]"]/*' />
    FileAttributes GetFileAttributes(EntryPath path);

    /// <include file='Docs/IFileIO.xml' path='IFileIO/Methods/Instance[@name="SetFileAttributes[EntryPath,FileAttributes]"]/*' />
    void SetFileAttributes(EntryPath path, FileAttributes attributes);
}

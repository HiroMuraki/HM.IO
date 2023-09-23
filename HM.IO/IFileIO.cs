namespace HM.IO;

public interface IFileIO
{
    Boolean Exists(EntryPath path);

    Stream OpenRead(EntryPath path);

    Stream OpenWrite(EntryPath path);

    void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath);

    void Delete(EntryPath path);

    FileTimestamps GetFileTimestamps(EntryPath path);

    void SetFileTimestamps(EntryPath path, FileTimestamps timestamps);

    FileAttributes GetFileAttributes(EntryPath path);

    void SetFileAttributes(EntryPath path, FileAttributes attributes);
}

namespace HM.IO;

public interface IFileIO
{
    Boolean Exists(EntryPath path);

    Stream OpenRead(EntryPath path);

    Stream OpenWrite(EntryPath path);

    void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath);

    void Delete(EntryPath path);

    EntryTimestamps GetFileTimestamps(EntryPath path);

    void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps);

    FileAttributes GetFileAttributes(EntryPath path);

    void SetFileAttributes(EntryPath path, FileAttributes attributes);
}

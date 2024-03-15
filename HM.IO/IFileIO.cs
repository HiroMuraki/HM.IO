namespace HM.IO;

public interface IFileIO
{
    static abstract Boolean Exists(EntryPath path);

    static abstract Stream OpenRead(EntryPath path);

    static abstract Stream OpenWrite(EntryPath path);

    static abstract void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath);

    static abstract void Delete(EntryPath path);

    static abstract EntryTimestamps GetFileTimestamps(EntryPath path);

    static abstract void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps);

    static abstract FileAttributes GetFileAttributes(EntryPath path);

    static abstract void SetFileAttributes(EntryPath path, FileAttributes attributes);
}

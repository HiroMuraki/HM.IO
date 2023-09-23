namespace HM.IO;

public interface IFileMetaDataManipulator
{
    DateTime GetCreationTime(EntryPath entryPath);

    void SetCreationTime(EntryPath entryPath, DateTime dateTime);

    DateTime GetLastWriteTime(EntryPath entryPath);

    void SetLastWriteTime(EntryPath entryPath, DateTime dateTime);

    DateTime GetLastAccessTime(EntryPath entryPath);

    void SetLastAccessTime(EntryPath entryPath, DateTime dateTime);

    FileAttributes GetFileAttributes(EntryPath entryPath);

    void SetFileAttributes(EntryPath entryPath, FileAttributes fileAttributes);
}

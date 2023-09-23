namespace HM.IO;

public class FileMetaDataManipulator : IFileMetaDataManipulator
{
    public DateTime GetCreationTime(EntryPath entryPath)
    {
        return File.GetCreationTime(entryPath.StringPath);
    }

    public FileAttributes GetFileAttributes(EntryPath entryPath)
    {
        return File.GetAttributes(entryPath.StringPath);
    }

    public DateTime GetLastAccessTime(EntryPath entryPath)
    {
        return File.GetLastAccessTime(entryPath.StringPath);
    }

    public DateTime GetLastWriteTime(EntryPath entryPath)
    {
        return File.GetLastWriteTime(entryPath.StringPath);
    }

    public void SetCreationTime(EntryPath entryPath, DateTime dateTime)
    {
        File.SetCreationTime(entryPath.StringPath, dateTime);
    }

    public void SetFileAttributes(EntryPath entryPath, FileAttributes fileAttributes)
    {
        File.SetAttributes(entryPath.StringPath, fileAttributes);
    }

    public void SetLastAccessTime(EntryPath entryPath, DateTime dateTime)
    {
        File.SetLastAccessTime(entryPath.StringPath, dateTime);
    }

    public void SetLastWriteTime(EntryPath entryPath, DateTime dateTime)
    {
        File.SetLastWriteTime(entryPath.StringPath, dateTime);
    }
}

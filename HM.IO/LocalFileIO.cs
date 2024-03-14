namespace HM.IO;

public class LocalFileIO : IFileIO
{
    public static LocalFileIO Default { get; } = new();

    public Boolean Exists(EntryPath filePath)
    {
        return File.Exists(filePath.StringPath);
    }

    public void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        File.Move(sourceFilePath.StringPath, destinationFilePath.StringPath);
    }

    public void Delete(EntryPath filePath)
    {
        File.Delete(filePath.StringPath);
    }

    public Stream OpenRead(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }

    public Stream OpenWrite(EntryPath filePath)
    {
        return File.OpenWrite(filePath.StringPath);
    }

    public EntryTimestamps GetFileTimestamps(EntryPath path)
    {
        String filePath = path.StringPath;

        return new EntryTimestamps
        {
            CreationTime = File.GetCreationTime(filePath),
            LastWriteTime = File.GetLastWriteTime(filePath),
            LastAccessTime = File.GetLastAccessTime(filePath),
        };
    }

    public void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps)
    {
        String filePath = path.StringPath;

        File.SetCreationTime(filePath, timestamps.CreationTime);
        File.SetLastWriteTime(filePath, timestamps.LastWriteTime);
        File.SetLastAccessTime(filePath, timestamps.LastAccessTime);
    }

    public FileAttributes GetFileAttributes(EntryPath path)
    {
        return File.GetAttributes(path.StringPath);
    }

    public void SetFileAttributes(EntryPath path, FileAttributes attributes)
    {
        File.SetAttributes(path.StringPath, attributes);
    }

    #region NonPublic
    private LocalFileIO()
    {

    }
    #endregion
}

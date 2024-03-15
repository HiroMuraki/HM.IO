namespace HM.IO;

public class LocalFileIO :
    IFileIO
{
    public static Boolean Exists(EntryPath filePath)
    {
        return File.Exists(filePath.StringPath);
    }

    public static void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        File.Move(sourceFilePath.StringPath, destinationFilePath.StringPath);
    }

    public static void Delete(EntryPath filePath)
    {
        File.Delete(filePath.StringPath);
    }

    public static Stream OpenRead(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }

    public static Stream OpenWrite(EntryPath filePath)
    {
        return File.OpenWrite(filePath.StringPath);
    }

    public static EntryTimestamps GetFileTimestamps(EntryPath path)
    {
        String filePath = path.StringPath;

        return new EntryTimestamps
        {
            CreationTime = File.GetCreationTime(filePath),
            LastWriteTime = File.GetLastWriteTime(filePath),
            LastAccessTime = File.GetLastAccessTime(filePath),
        };
    }

    public static void SetFileTimestamps(EntryPath path, EntryTimestamps timestamps)
    {
        String filePath = path.StringPath;

        File.SetCreationTime(filePath, timestamps.CreationTime);
        File.SetLastWriteTime(filePath, timestamps.LastWriteTime);
        File.SetLastAccessTime(filePath, timestamps.LastAccessTime);
    }

    public static FileAttributes GetFileAttributes(EntryPath path)
    {
        return File.GetAttributes(path.StringPath);
    }

    public static void SetFileAttributes(EntryPath path, FileAttributes attributes)
    {
        File.SetAttributes(path.StringPath, attributes);
    }

    #region NonPublic
    private LocalFileIO()
    {

    }
    #endregion
}

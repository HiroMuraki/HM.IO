namespace HM.IO;

public class FileIO : IFileIO
{
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
        return File.OpenRead(filePath.StringPath);
    }

    public FileTimestamps GetFileTimestamps(EntryPath path)
    {
        String filePath = path.StringPath;

        return new FileTimestamps
        {
            CreationTime = File.GetCreationTime(filePath),
            LastWriteTime = File.GetLastWriteTime(filePath),
            LastAccessTime = File.GetLastAccessTime(filePath),
        };
    }

    public void SetFileTimestamps(EntryPath path, FileTimestamps timestamps)
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
}

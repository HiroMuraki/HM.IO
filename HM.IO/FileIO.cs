namespace HM.IO;

/// <include file='Docs/FileIO.xml' path='FileIO/Class[@name="FileIO"]/*' />
public class FileIO : IFileIO
{
    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    public Boolean Exists(EntryPath filePath)
    {
        return File.Exists(filePath.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="Rename[EntryPath,EntryPath]"]/*' />
    public void Rename(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        File.Move(sourceFilePath.StringPath, destinationFilePath.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="Delete[EntryPath]"]/*' />
    public void Delete(EntryPath filePath)
    {
        File.Delete(filePath.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="OpenRead[EntryPath]"]/*' />
    public Stream OpenRead(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="OpenWrite[EntryPath]"]/*' />
    public Stream OpenWrite(EntryPath filePath)
    {
        return File.OpenWrite(filePath.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="GetFileTimestamps[EntryPath]"]/*' />
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

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="SetFileTimestamps[EntryPath,FileTimestamps]"]/*' />
    public void SetFileTimestamps(EntryPath path, FileTimestamps timestamps)
    {
        String filePath = path.StringPath;

        File.SetCreationTime(filePath, timestamps.CreationTime);
        File.SetLastWriteTime(filePath, timestamps.LastWriteTime);
        File.SetLastAccessTime(filePath, timestamps.LastAccessTime);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="GetFileAttributes[EntryPath]"]/*' />
    public FileAttributes GetFileAttributes(EntryPath path)
    {
        return File.GetAttributes(path.StringPath);
    }

    /// <include file='Docs/FileIO.xml' path='FileIO/Methods/Instance[@name="SetFileAttributes[EntryPath,FileAttributes]"]/*' />
    public void SetFileAttributes(EntryPath path, FileAttributes attributes)
    {
        File.SetAttributes(path.StringPath, attributes);
    }
}

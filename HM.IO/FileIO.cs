namespace HM.IO;

public class FileIO : IFileIO
{
    public void Delete(EntryPath filePath)
    {
        File.Delete(filePath.StringPath);
    }

    public Boolean Exists(EntryPath filePath)
    {
        return File.Exists(filePath.StringPath);
    }

    public void Move(EntryPath sourceFilePath, EntryPath destinationFilePath)
    {
        File.Move(sourceFilePath.StringPath, destinationFilePath.StringPath);
    }

    public Stream OpenRead(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }

    public Stream OpenWrite(EntryPath filePath)
    {
        return File.OpenRead(filePath.StringPath);
    }
}

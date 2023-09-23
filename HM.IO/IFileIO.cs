namespace HM.IO;

public interface IFileIO
{
    Boolean Exists(EntryPath path);

    Stream OpenRead(EntryPath path);

    Stream OpenWrite(EntryPath path);

    void Move(EntryPath sourceFilePath, EntryPath destinationFilePath);

    void Delete(EntryPath path);
}

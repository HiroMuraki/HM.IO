namespace HM.IO;

public interface IAsyncFileProcessor
{
    Task ProcessAsync(EntryPath entryPath);
}

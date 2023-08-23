namespace HM.IO;

public interface IFilesProvider : IItemsProvider<EntryPath>
{
    IEnumerable<EntryPath> EnumerateFiles();
}

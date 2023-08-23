namespace HM.IO;

public interface IDiretoriesProvider : IItemsProvider<EntryPath>
{
    IEnumerable<EntryPath> EnumerateDirectories();
}

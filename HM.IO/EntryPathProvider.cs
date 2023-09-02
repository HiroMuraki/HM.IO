namespace HM.IO;

public abstract class EntryPathProvider :
    IItemsProvider<EntryPath>
{
    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();

    public abstract IEnumerable<EntryPath> EnumerateItems();

    #region NonPublic
    protected static List<EntryPath> SelectNotEmptyAsDistinctEntryPath(IEnumerable<String> items)
    {
        return items
            .Where(x => !String.IsNullOrWhiteSpace(x))
            .Select(EntryPath.CreateFromPath)
            .Distinct()
            .ToList();
    }
    #endregion
}

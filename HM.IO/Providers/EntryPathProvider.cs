namespace HM.IO.Providers;

public abstract class EntryPathProvider :
    IItemsProvider<EntryPath>
{
    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();

    public abstract IEnumerable<EntryPath> EnumerateItems();
}

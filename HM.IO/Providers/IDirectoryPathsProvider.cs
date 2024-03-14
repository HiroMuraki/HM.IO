namespace HM.IO.Providers;

public interface IDirectoryPathsProvider
{
    IEnumerable<EntryPath> EnumerateDirectoryPaths();
}

namespace HM.IO.Providers;

public interface IFilePathsProvider
{
    IEnumerable<EntryPath> EnumerateFilePaths();
}

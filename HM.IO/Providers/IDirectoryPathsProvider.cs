namespace HM.IO.Providers;

/// <include file='Providers/IDirectoryPathsProvider.xml' path='IDirectoryPathsProvider/Class[@name="IDirectoryPathsProvider"]/*' />
public interface IDirectoryPathsProvider
{
    /// <include file='Providers/IDirectoryPathsProvider.xml' path='IDirectoryPathsProvider/Methods/Instance[@name="EnumerateDirectoryPaths[]"]/*' />
    IEnumerable<EntryPath> EnumerateDirectoryPaths();
}

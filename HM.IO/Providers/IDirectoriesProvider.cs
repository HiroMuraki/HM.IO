namespace HM.IO.Providers;

/// <include file='Docs/Providers/IDirectoriesProvider.xml' path='IDirectoriesProvider/Class[@name="IDirectoriesProvider"]/*' />
public interface IDirectoriesProvider
{
    /// <include file='Docs/Providers/IDirectoriesProvider.xml' path='IDirectoriesProvider/Methods/Instance[@name="EnumerateDirectories[]"]/*' />
    IEnumerable<EntryPath> EnumerateDirectories();
}

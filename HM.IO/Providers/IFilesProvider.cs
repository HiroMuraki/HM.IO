namespace HM.IO.Providers;

/// <include file='Docs/Providers/IFilesProvider.xml' path='IFilesProvider/Class[@name="IFilesProvider"]/*' />
public interface IFilesProvider
{
    /// <include file='Docs/Providers/IFilesProvider.xml' path='IFilesProvider/Methods/Instance[@name="EnumerateFiles[]"]/*' />
    IEnumerable<EntryPath> EnumerateFilePaths();
}

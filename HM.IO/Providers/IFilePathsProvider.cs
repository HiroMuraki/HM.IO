namespace HM.IO.Providers;

/// <include file='Providers/IFilePathsProvider.xml' path='IFilePathsProvider/Class[@name="IFilePathsProvider"]/*' />
public interface IFilePathsProvider
{
    /// <include file='Providers/IFilePathsProvider.xml' path='IFilePathsProvider/Methods/Instance[@name="EnumerateFilePaths[]"]/*' />
    IEnumerable<EntryPath> EnumerateFilePaths();
}

namespace HM.IO;

/// <include file='Docs/IFileHashComputer.xml' path='IFileHashComputer/Class[@name="IFileHashComputer"]/*' />
public interface IFileHashComputer
{
    /// <include file='Docs/IFileHashComputer.xml' path='IFileHashComputer/Methods/Instance[@name="ComputeHashAsync[EntryPath]"]/*' />
    Task<String> ComputeHashAsync(EntryPath filePath);
}

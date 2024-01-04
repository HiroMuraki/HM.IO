namespace HM.IO;

/// <include file='IFileHashComputer.xml' path='IFileHashComputer/Class[@name="IFileHashComputer"]/*' />
public interface IFileHashComputer
{
    /// <include file='IFileHashComputer.xml' path='IFileHashComputer/Methods/Instance[@name="ComputeHashAsync[EntryPath]"]/*' />
    Task<String> ComputeHashAsync(EntryPath filePath);
}

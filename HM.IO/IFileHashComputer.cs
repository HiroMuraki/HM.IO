namespace HM.IO;

/// <include file='IFileHashComputer.xml' path='IFileHashComputer/Class[@name="IFileHashComputer"]/*' />
public interface IFileHashComputer
{
    /// <include file='IFileHashComputer.xml' path='IFileHashComputer/Methods/Instance[@name="ComputeHashAsync[EntryPath]"]/*' />
    Task<String> ComputeHashAsync(EntryPath filePath);
}

public static class FileHashComputerExtension
{
    public static async Task<String> ComputeHashAsync(this IFileHashComputer hashComputer, String filePath)
        => await hashComputer.ComputeHashAsync(EntryPath.Create(filePath));
}
#if OLD2
namespace HM.IO;

/// <summary>
/// 	Interface defining a contract for computing hash values for files asynchronously.
/// </summary>
public interface IFileHashComputer
{
    /// <Instance name="ComputeHashAsync[EntryPath]">
    /// 	<summary>
    /// 		Computes the hash value for the specified file asynchronously.
    /// 	</summary>
    /// 	<param name="filePath">The path of the file for which to compute the hash.</param>
    /// 	<returns>
    /// 		A task representing the asynchronous operation. The task result is the computed hash value.
    /// 	</returns>
    /// </Instance>
    Task<String> ComputeHashAsync(EntryPath filePath);
}

public static class FileHashComputerExtension
{
    public static async Task<String> ComputeHashAsync(this IFileHashComputer hashComputer, String filePath)
        => await hashComputer.ComputeHashAsync(EntryPath.Create(filePath));
}
#endif
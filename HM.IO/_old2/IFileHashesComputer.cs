#if OLD2
using System.Collections.Immutable;

namespace HM.IO;

/// <summary>
/// 	Interface defining a contract for computing hash values for multiple files asynchronously.
/// </summary>
public interface IFileHashesComputer
{
    /// <summary>
    /// 	Computes the hash values for the specified collection of files asynchronously.
    /// </summary>
    /// <param name="filePaths">The collection of file paths for which to compute the hash values.</param>
    /// <returns>
    /// 	A task representing the asynchronous operation. The task result is a dictionary mapping file paths to their respective computed hash values.
    /// </returns>
    Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths);
}
#endif
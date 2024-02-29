using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace HM.IO;

/// <summary>
/// 	Default implementation for computing SHA-256 hash values for files.
/// </summary>
public sealed class FileSha256Computer : IFileHashComputer, IFileHashesComputer
{
    /// <summary>
    /// 	Computes the SHA-256 hash value for the specified file asynchronously.
    /// </summary>
    /// <param name="filePath">The path of the file for which to compute the hash.</param>
    /// <returns>
    /// 	A task representing the asynchronous operation. The task result is the computed SHA-256 hash value.
    /// </returns>
    public async Task<String> ComputeHashAsync(EntryPath filePath)
    {
        using var sha256 = SHA256.Create();
        using FileStream fs = File.OpenRead(filePath.StringPath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(fs));
    }

    /// <summary>
    /// 	Computes the SHA-256 hash values for the specified collection of files asynchronously.
    /// </summary>
    /// <param name="filePaths">The collection of file paths for which to compute the hash values.</param>
    /// <returns>
    /// 	A task representing the asynchronous operation. The task result is a dictionary mapping file paths to their respective computed SHA-256 hash values.
    /// </returns>
    public Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths)
    {
        return ComputeHashesAsync(filePaths, false);
    }
   
    /// <summary>
    /// 	Computes the SHA-256 hash values for the specified collection of files asynchronously, optionally using tasks for parallel processing.
    /// </summary>
    /// <param name="filePaths">The collection of file paths for which to compute the hash values.</param>
    /// <param name="useTasks">A flag indicating whether to use tasks for parallel processing.</param>
    /// <returns>
    /// 	A task representing the asynchronous operation. The task result is a dictionary mapping file paths to their respective computed SHA-256 hash values.
    /// </returns>
    public async Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths, Boolean useTasks)
    {
        if (useTasks)
        {
            Int32 cpuCount = Environment.ProcessorCount;
            var result = new Dictionary<EntryPath, String>();

            foreach (EntryPath[] block in filePaths.Chunk(cpuCount))
            {
                Int32 taskCount = cpuCount <= block.Length ? cpuCount : block.Length;
                var tasks = new Task<String>[taskCount];

                for (Int32 i = 0; i < taskCount; i++)
                {
                    tasks[i] = ComputeHashAsync(block[i]);
                }

                String[] hashes = await Task.WhenAll(tasks);

                for (Int32 i = 0; i < taskCount; i++)
                {
                    result[block[i]] = hashes[i];
                }
            }

            return result.ToImmutableDictionary();

        }
        else
        {
            var result = new ConcurrentDictionary<EntryPath, String>();

            await Parallel.ForEachAsync(filePaths, ComputeFileHashAsync);

            return result.ToImmutableDictionary();

            async ValueTask ComputeFileHashAsync(EntryPath filePath, CancellationToken cancellationToken)
            {
                String hash = await ComputeHashAsync(filePath);

                result[filePath] = hash;
            }
        }
    }
}
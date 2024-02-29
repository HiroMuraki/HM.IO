using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace HM.IO;

/// <include file='FileSha256Computer.xml' path='FileSha256Computer/Class[@name="FileSha256Computer"]/*' />
public sealed class FileSha256Computer : IFileHashComputer, IFileHashesComputer
{
    /// <include file='FileSha256Computer.xml' path='FileSha256Computer/Methods/Instance[@name="ComputeHashAsync[EntryPath]"]/*' />
    public async Task<String> ComputeHashAsync(EntryPath filePath)
    {
        using var sha256 = SHA256.Create();
        using FileStream fs = File.OpenRead(filePath.StringPath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(fs));
    }

    /// <include file='FileSha256Computer.xml' path='FileSha256Computer/Methods/Instance[@name="ComputeHashesAsync[IEnumerable&lt;EntryPath&gt;]"]/*' />
    public Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths)
    {
        return ComputeHashesAsync(filePaths, false);
    }

    /// <include file='FileSha256Computer.xml' path='FileSha256Computer/Methods/Instance[@name="ComputeHashesAsync[IEnumerable&lt;EntryPath&gt;,Boolean]"]/*' />
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
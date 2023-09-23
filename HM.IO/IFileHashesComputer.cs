using System.Collections.Immutable;

namespace HM.IO;

public interface IFileHashesComputer
{
    Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths);
}
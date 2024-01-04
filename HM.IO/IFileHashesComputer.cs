using System.Collections.Immutable;

namespace HM.IO;

/// <include file='IFileHashesComputer.xml' path='IFileHashesComputer/Class[@name="IFileHashesComputer"]/*' />
public interface IFileHashesComputer
{
    /// <include file='IFileHashesComputer.xml' path='IFileHashesComputer/Methods/Instance[@name="ComputeHashesAsync[IEnumerable&lt;EntryPath&gt;]"]/*' />
    Task<ImmutableDictionary<EntryPath, String>> ComputeHashesAsync(IEnumerable<EntryPath> filePaths);
}
namespace HM.IO;

/// <summary>
/// Represents an interface for providing directory paths.
/// </summary>
public interface IDiretoriesProvider : IItemsProvider<EntryPath>
{
    /// <summary>
    /// Enumerates and returns a collection of paths to directories.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing directory paths.</returns>
    IEnumerable<EntryPath> EnumerateDirectories();
}

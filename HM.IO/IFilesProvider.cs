namespace HM.IO;

/// <summary>
/// Represents an interface for providing file paths.
/// </summary>
public interface IFilesProvider : IItemsProvider<EntryPath>
{
    /// <summary>
    /// Enumerates and returns a collection of paths to files.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing file paths.</returns>
    IEnumerable<EntryPath> EnumerateFiles();
}

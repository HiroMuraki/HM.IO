#if PREVIEW
namespace HM.IO.Previews.Generic;

/// <summary>
/// Represents an interface for providing file paths.
/// </summary>
public interface IFilesProvider<TEntryPath>
{
    /// <summary>
    /// Enumerates and returns a collection of paths to files.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="TEntryPath"/> instances representing file paths.</returns>
    IEnumerable<TEntryPath> EnumerateFiles();
}

#endif
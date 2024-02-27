#if PREVIEW
namespace HM.IO.Previews.Generic;

/// <summary>
/// Represents a interface for performing directory-related I/O operations.
/// </summary>
public interface IDirectoryIO<TEntryPath>
{
    /// <summary>
    /// Enumerates directories in the specified path with the given options.
    /// </summary>
    /// <param name="path">The path to enumerate directories from.</param>
    /// <param name="enumerationOptions">Options to control the directory enumeration.</param>
    /// <returns>An enumerable collection of directory <see cref="TEntryPath"/>.</returns>
    IEnumerable<TEntryPath> EnumerateFiles(TEntryPath path, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Enumerates files in the specified path with the given options.
    /// </summary>
    /// <param name="path">The path to enumerate files from.</param>
    /// <param name="enumerationOptions">Options to control the file enumeration.</param>
    /// <returns>An enumerable collection of file <see cref="TEntryPath"/>.</returns>
    IEnumerable<TEntryPath> EnumerateDirectories(TEntryPath path, EnumerationOptions enumerationOptions);
}

#endif
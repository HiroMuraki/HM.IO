#if PREVIEW
using System.Collections.Immutable;

namespace HM.IO.Previews.Generic;
/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider :
    EntryPathProvider,
    IFilesProvider<EntryPath>
{
    /// <summary>
    /// Gets the list of directory paths to include during enumeration.
    /// </summary>
    public List<String> IncludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to include during enumeration.
    /// </summary>
    public List<String> IncludingFiles { get; init; } = new();

    /// <summary>
    /// Gets the list of directory paths to exclude during enumeration.
    /// </summary>
    public List<String> ExcludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to exclude during enumeration.
    /// </summary>
    public List<String> ExcludingFiles { get; init; } = new();

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateFiles()
    {
        var includingFiles = SelectNotEmptyAsDistinctEntryPath(IncludingFiles);
        var excludingFiles = SelectNotEmptyAsDistinctEntryPath(ExcludingFiles).ToImmutableHashSet();
        var enumeratioinOptons = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false,
            AttributesToSkip = FileAttributes.Normal,
        };
        var directoriesProvider = new DirectoriesProvider()
        {
            IncludingDirectories = IncludingDirectories,
            ExcludingDirectories = ExcludingDirectories
        };

        foreach (var file in includingFiles)
        {
            if (CanInclude(file))
            {
                yield return file;
            }
        }
        foreach (var directory in directoriesProvider.EnumerateDirectories())
        {
            foreach (var file in DirectoryIO.EnumerateFiles(directory, enumeratioinOptons))
            {
                if (CanInclude(file))
                {
                    yield return file;
                }
            }
        }

        Boolean CanInclude(EntryPath path)
        {
            return !excludingFiles.Any(e => e == path);
        }
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateFiles();
    }
}

#endif
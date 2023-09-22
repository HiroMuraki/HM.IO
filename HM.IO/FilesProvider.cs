using System.Collections.Immutable;

namespace HM.IO;

/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider :
    EntryPathProvider,
    IFilesProvider
{
    /// <summary>
    /// Gets the list of directory paths to include during enumeration.
    /// </summary>
    public List<SearchingDirectory> IncludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of directory paths to exclude during enumeration.
    /// </summary>
    public List<SearchingDirectory> ExcludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to include during enumeration.
    /// </summary>
    public List<SearchingFile> IncludingFiles { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to exclude during enumeration.
    /// </summary>
    public List<SearchingFile> ExcludingFiles { get; init; } = new();

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateFiles()
    {
        IEnumerable<EntryPath> includingFiles = IncludingFiles
            .Where(x => !String.IsNullOrEmpty(x.FilePath.StringPath))
            .Select(x => x.FilePath)
            .Distinct();
        var excludingFiles = ExcludingFiles
            .Where(x => !String.IsNullOrEmpty(x.FilePath.StringPath))
            .Select(x => x.FilePath)
            .ToImmutableHashSet();

        foreach (EntryPath file in includingFiles)
        {
            if (CanInclude(file))
            {
                yield return file;
            }
        }

        var enumeratioinOptons = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false,
        };
        var directoriesProvider = new DirectoriesProvider()
        {
            IncludingDirectories = IncludingDirectories,
            ExcludingDirectories = ExcludingDirectories
        };
        var directotires = directoriesProvider.EnumerateDirectories().ToList();

        foreach (EntryPath directory in directotires)
        {
            foreach (EntryPath file in DirectoryIO.EnumerateFiles(directory, enumeratioinOptons))
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

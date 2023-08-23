using System.Collections.Immutable;

namespace HM.IO;

/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider : IFilesProvider
{
    /// <summary>
    /// Gets or sets the directory I/O provider.
    /// </summary>
    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();
    /// <summary>
    /// Gets the equality comparer for comparing <see cref="EntryPath"/> instances.
    /// </summary>
    public IEqualityComparer<EntryPath> EntryPathEqualityComparer { get; } = IO.EntryPathEqualityComparer.Default;
    /// <summary>
    /// Gets the comparer for sorting <see cref="EntryPath"/> instances.
    /// </summary>
    public IComparer<EntryPath> EntryPathComparer { get; } = IO.EntryPathComparer.Default;
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
        var directoriesProvider = new DirectoriesProvider()
        {
            IncludingDirectories = IncludingDirectories,
            ExcludingDirectories = ExcludingDirectories
        };
        var searchingDirectories = directoriesProvider.EnumerateDirectories();

        var includingFiles = SelectAsEntryPath(IncludingFiles);
        var excludingFiles = SelectAsEntryPath(ExcludingFiles);
        //Get files from searching directories
        var files = searchingDirectories
            .SelectMany(d =>
            {
                return DirectoryIO
                    .EnumerateFiles(d, new EnumerationOptions()
                    {
                        IgnoreInaccessible = false,
                        RecurseSubdirectories = false,
                        AttributesToSkip = FileAttributes.Normal,
                    });
            })
            .Select(EntryPath.CreateFromPath)
            .Concat(includingFiles)
            .Except(excludingFiles, EntryPathEqualityComparer)
            .ToImmutableHashSet(EntryPathEqualityComparer);

        foreach (var file in files.Order(EntryPathComparer))
        {
            yield return file;
        }
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateFiles();
    }

    #region NonPublic
    private static List<EntryPath> SelectAsEntryPath(IEnumerable<String> items)
    {
        return items
            .Where(x => !String.IsNullOrWhiteSpace(x))
            .Select(EntryPath.CreateFromPath)
            .ToList();
    }
    #endregion
}

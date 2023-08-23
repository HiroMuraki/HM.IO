using System.Collections.Immutable;

namespace HM.IO;

public sealed class FilesProvider : IFilesProvider
{
    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();
    public IEqualityComparer<EntryPath> EntryPathEqualityComparer { get; } = IO.EntryPathEqualityComparer.Default;
    public IComparer<EntryPath> EntryPathComparer { get; } = IO.EntryPathComparer.Default;
    public List<String> IncludingDirectories { get; init; } = new();
    public List<String> IncludingFiles { get; init; } = new();
    public List<String> ExcludingDirectories { get; init; } = new();
    public List<String> ExcludingFiles { get; init; } = new();

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

public sealed class DirectoriesProvider : IDiretoriesProvider
{
    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();
    public IEqualityComparer<EntryPath> EntryPathEqualityComparer { get; } = IO.EntryPathEqualityComparer.Default;
    public IComparer<EntryPath> EntryPathComparer { get; } = IO.EntryPathComparer.Default;
    public List<String> IncludingDirectories { get; init; } = new();
    public List<String> ExcludingDirectories { get; init; } = new();

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        var includingDirectories = SelectAsEntryPath(IncludingDirectories);
        var excludingDirectories = SelectAsEntryPath(ExcludingDirectories);

        var directoryEnumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Normal,
        };

        // Get directories to search
        var normalDirectories = includingDirectories
            .Where(d => d.Routes[^1] != "*").ToList();
        var recursiveDirectories = includingDirectories
            .Except(normalDirectories, EntryPathEqualityComparer)
            .Select(d => d[0..^1]);
        var subDirectories = recursiveDirectories
            .SelectMany(d => DirectoryIO.EnumerateDirectories(d, directoryEnumerationOptions))
            .Select(EntryPath.CreateFromPath);
        var directories = normalDirectories
            .Concat(recursiveDirectories)
            .Concat(subDirectories)
            .Except(excludingDirectories, EntryPathEqualityComparer)
            .ToImmutableHashSet(EntryPathEqualityComparer);

        foreach (var directory in directories.Order(EntryPathComparer))
        {
            yield return directory;
        }
    }

    public IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateDirectories();
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
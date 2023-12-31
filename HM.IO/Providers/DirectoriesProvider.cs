using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Providers;

public sealed class DirectoriesProvider :
    EntryPathsProvider,
    IDirectoriesProvider
{
    public List<SearchingDirectory> IncludingDirectories { get; init; } = new();

    public List<SearchingDirectory> ExcludingDirectories { get; init; } = new();

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        IEnumerable<EntryPath> includingDirectories = IncludingDirectories
            .Where(x => !String.IsNullOrWhiteSpace(x.BaseDirectory.StringPath))
            .DistinctBy(x => x.BaseDirectory)
            .SelectMany(EnumerateDirectories);

        var excludingDirectories = ExcludingDirectories
            .Where(x => !String.IsNullOrWhiteSpace(x.BaseDirectory.StringPath))
            .DistinctBy(x => x.BaseDirectory)
            .SelectMany(EnumerateDirectories)
            .ToImmutableHashSet();

        foreach (EntryPath directory in includingDirectories)
        {
            if (!excludingDirectories.Contains(directory))
            {
                yield return directory;
            }
        }
    }

    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateDirectories();
    }

    #region NonPublic
    private IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory directory)
    {
        if (!DirectoryIO.Exists(directory.BaseDirectory))
        {
            if (directory.IgnoreIfNotExists)
            {
                yield break;
            }
            else
            {
                throw new DirectoryNotFoundException(directory.BaseDirectory.StringPath);
            }
        }

        IEnumerable<EntryPath> directories = DirectoryIO.EnumerateDirectories(directory.BaseDirectory, new EnumerationOptions
        {
            RecurseSubdirectories = directory.RecurseSubdirectories,
            MaxRecursionDepth = directory.MaxRecursionDepth,
        });

        yield return directory.BaseDirectory;

        foreach (EntryPath item in directories)
        {
            yield return item;
        }
    }
    #endregion
}

public sealed class DirectoriesProviderX :
    EntryPathsProvider,
    IDirectoriesProvider
{
    public DirectoriesProviderX IncludeDirectory(EntryPath path)
    {
        return AddPathHelper(_includingDirectories, ref path);
    }

    public DirectoriesProviderX ExcludeDirectory(EntryPath path)
    {
        return AddPathHelper(_excludingDirectories, ref path);
    }

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateDirectories();
    }

    #region NonPublic
    private readonly List<EntryPath> _includingDirectories = [];
    private readonly List<EntryPath> _excludingDirectories = [];
    private DirectoriesProviderX AddPathHelper(List<EntryPath> list, ref EntryPath entryPath)
    {
        if (!list.Contains(entryPath))
        {
            list.Add(entryPath);
        }

        return this;
    }
    #endregion
}

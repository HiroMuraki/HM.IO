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
            .Where(x => !String.IsNullOrWhiteSpace(x.Path.StringPath))
            .DistinctBy(x => x.Path)
            .SelectMany(EnumerateDirectories);

        var excludingDirectories = ExcludingDirectories
            .Where(x => !String.IsNullOrWhiteSpace(x.Path.StringPath))
            .DistinctBy(x => x.Path)
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
        if (!DirectoryIO.Exists(directory.Path))
        {
            if (directory.IgnoreIfNotExists)
            {
                yield break;
            }
            else
            {
                throw new DirectoryNotFoundException(directory.Path.StringPath);
            }
        }

        IEnumerable<EntryPath> directories = DirectoryIO.EnumerateDirectories(directory.Path, new EnumerationOptions
        {
            RecurseSubdirectories = directory.RecurseSubdirectories,
            MaxRecursionDepth = directory.MaxRecursionDepth,
        });

        yield return directory.Path;

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
    public DirectoriesProviderX IncludeDirectory(SearchingDirectory path)
    {
        return (DirectoriesProviderX)AddOptionHelper(_includingDirectories, ref path);
    }

    public DirectoriesProviderX ExcludeDirectory(SearchingDirectory path)
    {
        return (DirectoriesProviderX)AddOptionHelper(_excludingDirectories, ref path);
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
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    #endregion
}

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public sealed class DirectoriesProvider :
    EntryPathProvider,
    IDirectoriesProvider
{
    public List<SearchingDirectory> IncludingDirectories { get; init; } = new();

    public List<SearchingDirectory> ExcludingDirectories { get; init; } = new();

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        IEnumerable<EntryPath> includingDirectories = IncludingDirectories
            .Where(x => !String.IsNullOrWhiteSpace(x.BaseDirectroy.StringPath))
            .DistinctBy(x => x.BaseDirectroy)
            .SelectMany(EnumerateDirectories);

        var excludingDirectories = ExcludingDirectories
            .Where(x => !String.IsNullOrWhiteSpace(x.BaseDirectroy.StringPath))
            .DistinctBy(x => x.BaseDirectroy)
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
        if (!DirectoryIO.Exists(directory.BaseDirectroy))
        {
            if (directory.IgnoreIfNotExists)
            {
                yield break;
            }
            else
            {
                throw new DirectoryNotFoundException(directory.BaseDirectroy.StringPath);
            }
        }

        IEnumerable<EntryPath> directories = DirectoryIO.EnumerateDirectories(directory.BaseDirectroy, new EnumerationOptions
        {
            RecurseSubdirectories = directory.RecurseSubdirectories,
            MaxRecursionDepth = directory.MaxRecursionDepth,
        });

        yield return directory.BaseDirectroy;

        foreach (EntryPath item in directories)
        {
            yield return item;
        }
    }
    #endregion
}
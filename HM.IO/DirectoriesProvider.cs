using System.Collections.Immutable;

namespace HM.IO;

public sealed class DirectoriesProvider :
    EntryPathProvider,
    IDirectoriesProvider
{
    public List<String> IncludingDirectories { get; init; } = new();

    public List<String> ExcludingDirectories { get; init; } = new();

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        var includingDirectories = SelectNotEmptyAsEntryPath(IncludingDirectories)
            .ToImmutableHashSet(EntryPathComparer);
        var excludingDirectories = SelectNotEmptyAsEntryPath(ExcludingDirectories);
        var recursiveExcludingDirectories = excludingDirectories
            .Where(IsRecursiveDirectory)
            .Select(GetRecursiveDirectory)
            .ToImmutableHashSet(EntryPathComparer);
        var normalExcludingDirectories = excludingDirectories
            .Where(d => !IsRecursiveDirectory(d))
            .Concat(recursiveExcludingDirectories)
            .ToImmutableHashSet(EntryPathComparer);

        var enumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Normal,
        };

        var selectedDirectories = new HashSet<CompressedEntryPath>();
        var entryPathCompressor = new EntryPathCompressor();
        foreach (var directory in includingDirectories)
        {
            if (IsRecursiveDirectory(directory))
            {
                var recursivelyDirectory = GetRecursiveDirectory(directory);
                if (CanIncluded(directory))
                {
                    selectedDirectories.Add(entryPathCompressor.Compress(recursivelyDirectory));
                }

                var subDirectories = DirectoryIO.EnumerateDirectories(recursivelyDirectory, enumerationOptions);
                foreach (var subDirectory in subDirectories)
                {
                    if (CanIncluded(subDirectory))
                    {
                        selectedDirectories.Add(entryPathCompressor.Compress(subDirectory));
                    }
                }
            }
            else if (CanIncluded(directory))
            {
                selectedDirectories.Add(entryPathCompressor.Compress(directory));
            }
        }

        foreach (var directory in selectedDirectories)
        {
            yield return entryPathCompressor.Restore(directory);
        }

        Boolean CanIncluded(EntryPath path)
        {
            if (normalExcludingDirectories.Contains(path))
            {
                return false;
            }
            if (recursiveExcludingDirectories.Any(e => e == path || path.IsSubPathOf(e, EntryPathComparer.RouteEqualityComparer)))
            {
                return false;
            }

            return true;
        }
    }

    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateDirectories();
    }

    #region NonPublic
    private Boolean IsRecursiveDirectory(EntryPath path)
    {
        // If last char of path is "*", indicating that should enumerate its sub directories.
        return path[^1] == "*";
    }
    private EntryPath GetRecursiveDirectory(EntryPath path)
    {
        return path[0..(path.Length - 1)];
    }
    #endregion
}
using System.Collections.Immutable;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace HM.IO;

public sealed class DirectoriesProvider
    : EntryPathProvider, IDiretoriesProvider
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

        var selectedDirectories = new HashSet<EntryPath>(EntryPathComparer);
        foreach (var directory in includingDirectories)
        {
            if (IsRecursiveDirectory(directory))
            {
                var recursivelyDirectory = GetRecursiveDirectory(directory);
                if (CanIncluded(directory))
                {
                    selectedDirectories.Add(recursivelyDirectory);
                }

                var subDirectories = DirectoryIO.EnumerateDirectories(recursivelyDirectory, enumerationOptions);
                foreach (var subDirectory in subDirectories)
                {
                    if (CanIncluded(subDirectory))
                    {
                        selectedDirectories.Add(subDirectory);
                    }
                }
            }
            else if (CanIncluded(directory))
            {
                selectedDirectories.Add(directory);
            }
        }

        foreach (var directory in selectedDirectories.Order(EntryPathComparer))
        {
            yield return directory;
        }

        Boolean CanIncluded(EntryPath path)
        {
            if (normalExcludingDirectories.Contains(path))
            {
                return false;
            }
            if (recursiveExcludingDirectories.Any(e => e == path || path.IsSubPathOf(e, EntryPathComparer.RouteComparer)))
            {
                return false;
            }

            return true;
        }
        //var includingDirectories = SelectNotEmptyAsEntryPath(IncludingDirectories)
        //    .ToImmutableHashSet(EntryPathComparer);
        //var excludingDirectories = SelectNotEmptyAsEntryPath(ExcludingDirectories)
        //    .SelectMany(FetchDirectories)
        //    .ToImmutableHashSet(EntryPathComparer);

        //var selectedDirectories = includingDirectories
        //    .SelectMany(FetchDirectories)
        //    .ToHashSet(EntryPathComparer);
        //selectedDirectories.ExceptWith(excludingDirectories);

        //foreach (var directory in selectedDirectories.Order(EntryPathComparer))
        //{
        //    yield return directory;
        //}
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
        return path[0..(path.Routes.Length - 1)];
    }
    #endregion
}
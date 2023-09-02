using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public sealed class DirectoriesProvider :
    EntryPathProvider,
    IDirectoriesProvider
{
    public List<String> IncludingDirectories { get; init; } = new();

    public List<String> ExcludingDirectories { get; init; } = new();

    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        var includingDirectories = EnumerateDirectories(SelectNotEmptyAsDistinctEntryPath(IncludingDirectories));
        var excludingDirectories = EnumerateDirectories(SelectNotEmptyAsDistinctEntryPath(ExcludingDirectories))
            .ToImmutableHashSet();

        foreach (var directory in includingDirectories)
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
    private static Boolean TryAsRecursiveDirectory(EntryPath path, [NotNullWhen(true)] out EntryPath? recursiveDirectory)
    {
        if (path[^1] == "*")
        {
            recursiveDirectory = path[0..(path.Length - 1)];
            return true;
        }

        recursiveDirectory = null;
        return false;
    }
    private IEnumerable<EntryPath> EnumerateDirectories(List<EntryPath> paths)
    {
        var enumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.Normal,
        };

        foreach (var directory in paths)
        {
            if (TryAsRecursiveDirectory(directory, out EntryPath? recursiveDirectory))
            {
                yield return recursiveDirectory;

                var subDirectories = DirectoryIO.EnumerateDirectories(recursiveDirectory, enumerationOptions);
                foreach (var subDirectory in subDirectories)
                {
                    yield return subDirectory;
                }
            }
            else
            {
                yield return directory;
            }
        }
    }
    #endregion
}
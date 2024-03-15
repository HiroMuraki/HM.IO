using System.Collections.Immutable;

namespace HM.IO.Providers;

public sealed class LocalDirectoryPathsProvider
    : IDirectoryPathsProvider
{
    public static LocalDirectoryPathsProvider Create()
    {
        return new LocalDirectoryPathsProvider();
    }

    public LocalDirectoryPathsProvider IncludeDirectory(SearchingDirectory entryPath)
    {
        return AddOptionHelper(_includingDirectories, ref entryPath);
    }

    public LocalDirectoryPathsProvider ExcludeDirectory(SearchingDirectory entryPath)
    {
        return AddOptionHelper(_excludingDirectories, ref entryPath);
    }

    public IEnumerable<EntryPath> EnumerateDirectoryPaths()
    {
        var excludedDirectories = _excludingDirectories
            .SelectMany(EnumerateDirectories)
            .ToImmutableHashSet();

        return _includingDirectories
            .SelectMany(EnumerateDirectories)
            .SkipWhile(excludedDirectories.Contains);

        static IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory directory)
        {
            if (!LocalDirectoryIO.Exists(directory.Path))
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

            yield return directory.Path;

            if (directory.RecurseSubdirectories)
            {
                EnumerationOptions enumerationOptions = GetDirectoriesEnumerationOptions();
                enumerationOptions.RecurseSubdirectories = directory.RecurseSubdirectories;
                enumerationOptions.MaxRecursionDepth = directory.MaxRecursionDepth - 1;

                IEnumerable<EntryPath> directoryPaths = LocalDirectoryIO.EnumerateDirectoryPaths(
                    directory.Path, enumerationOptions);

                foreach (EntryPath path in directoryPaths)
                {
                    yield return path;
                }
            }
        }
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private static EnumerationOptions GetDirectoriesEnumerationOptions() => new()
    {
        IgnoreInaccessible = true,
        MatchType = MatchType.Simple,
        AttributesToSkip = (FileAttributes)Int32.MinValue,
    };
    private LocalDirectoryPathsProvider AddOptionHelper<T>(List<T> list, ref T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }

        return this;
    }
    private LocalDirectoryPathsProvider()
    {

    }
    #endregion
}

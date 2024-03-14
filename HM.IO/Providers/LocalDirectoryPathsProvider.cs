using System.Collections.Immutable;

namespace HM.IO.Providers;

public sealed class LocalDirectoryPathsProvider
    : IDirectoryPathsProvider
{
    public static LocalDirectoryPathsProvider Create()
    {
        return new LocalDirectoryPathsProvider();
    }

    public LocalDirectoryPathsProvider UseDirectoryIO(IDirectoryIO directoryIO)
    {
        ArgumentNullException.ThrowIfNull(directoryIO, nameof(directoryIO));

        _directoryIO = directoryIO;

        return this;
    }

    public LocalDirectoryPathsProvider UseErrorHandler(IErrorHandler errorHandler)
    {
        ArgumentNullException.ThrowIfNull(errorHandler, nameof(errorHandler));

        _errorHandler = errorHandler;

        return this;
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

        IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory directory)
        {
            if (!_directoryIO.Exists(directory.Path))
            {
                if (directory.IgnoreIfNotExists)
                {
                    yield break;
                }
                else
                {
                    HandleOrThrow(new DirectoryNotFoundException(directory.Path.StringPath));
                }
            }

            yield return directory.Path;

            if (directory.RecurseSubdirectories)
            {
                EnumerationOptions enumerationOptions = GetDirectoriesEnumerationOptions();
                enumerationOptions.RecurseSubdirectories = directory.RecurseSubdirectories;
                enumerationOptions.MaxRecursionDepth = directory.MaxRecursionDepth - 1;

                IEnumerable<EntryPath> directoryPaths = _directoryIO.EnumerateDirectoryPaths(
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
    private IDirectoryIO _directoryIO = LocalDirectoryIO.Default;
    private IErrorHandler? _errorHandler;
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
    private void HandleOrThrow<TException>(TException exception)
        where TException : Exception
    {
        if (_errorHandler?.Handle(exception) ?? false)
        {
            throw exception;
        }
    }
    private LocalDirectoryPathsProvider()
    {

    }
    #endregion
}

using System.Collections.Immutable;

namespace HM.IO.Providers;

/// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Class[@name="EntryPathsProvider"]/*' />
public sealed class EntryPathsProvider
    : IFilePathsProvider, IDirectoryPathsProvider
{
    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Static[@name="Create[]"]/*' />
    public static EntryPathsProvider Create()
    {
        return new EntryPathsProvider();
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="UseDirectoryIO[IDirectoryIO]"]/*' />
    public EntryPathsProvider UseDirectoryIO(IDirectoryIO directoryIO)
    {
        ArgumentNullException.ThrowIfNull(directoryIO, nameof(directoryIO));

        _directoryIO = directoryIO;

        return this;
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="UseErrorHandler[IErrorHandler]"]/*' />
    public EntryPathsProvider UseErrorHandler(IErrorHandler errorHandler)
    {
        ArgumentNullException.ThrowIfNull(errorHandler, nameof(errorHandler));

        _errorHandler = errorHandler;

        return this;
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="IncludeDirectory[SearchingDirectory]"]/*' />
    public EntryPathsProvider IncludeDirectory(SearchingDirectory entryPath)
    {
        return AddOptionHelper(_includingDirectories, ref entryPath);
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="ExcludeDirectory[SearchingDirectory]"]/*' />
    public EntryPathsProvider ExcludeDirectory(SearchingDirectory entryPath)
    {
        return AddOptionHelper(_excludingDirectories, ref entryPath);
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="IncludeFile[SearchingFile]"]/*' />
    public EntryPathsProvider IncludeFile(SearchingFile entryPath)
    {
        return AddOptionHelper(_includingFiles, ref entryPath);
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="ExcludeFile[SearchingFile]"]/*' />
    public EntryPathsProvider ExcludeFile(SearchingFile entryPath)
    {
        return AddOptionHelper(_excludingFiles, ref entryPath);
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="EnumerateFiles[]"]/*' />
    public IEnumerable<EntryPath> EnumerateFilePaths()
    {
        foreach (EntryPath entryPath in EnumerateCore())
        {
            yield return entryPath;
        }

        IEnumerable<EntryPath> EnumerateCore()
        {
            // Yield from files
            var includingFiles = _includingFiles
                .Where(x => !String.IsNullOrEmpty(x.Path.StringPath))
                .Select(x => x.Path)
                .ToList();
            var excludingFiles = _excludingFiles
                .Where(x => !String.IsNullOrEmpty(x.Path.StringPath))
                .Select(x => x.Path)
                .ToImmutableHashSet();

            foreach (EntryPath file in includingFiles)
            {
                if (CanInclude(file))
                {
                    yield return file;
                }
            }

            // yield from directories
            var excludedDirectories = _excludingDirectories
                .SelectMany(EnumerateDirectories)
                .ToImmutableHashSet();

            var includedDirectories = _includingDirectories
                .SelectMany(EnumerateDirectories)
                .Where(d => !excludedDirectories.Contains(d))
                .ToList();

            foreach (EntryPath directory in includedDirectories)
            {
                foreach (EntryPath file in _directoryIO.EnumerateFilePaths(directory, GetFilesEnumerationOptions()))
                {
                    if (CanInclude(file))
                    {
                        yield return file;
                    }
                }
            }

            Boolean CanInclude(EntryPath entryPath)
            {
                return !excludingFiles.Contains(entryPath);
            }

            IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory searchingDirectory)
            {
                yield return searchingDirectory.Path;

                if (searchingDirectory.RecurseSubdirectories && searchingDirectory.MaxRecursionDepth > 0)
                {
                    if (!_directoryIO.Exists(searchingDirectory.Path))
                    {
                        if (searchingDirectory.IgnoreIfNotExists)
                        {
                            yield break;
                        }
                        else
                        {
                            HandleOrThrow(new DirectoryNotFoundException(searchingDirectory.Path.StringPath));
                        }
                    }

                    EnumerationOptions enumerationOptions = GetDirectoriesEnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = searchingDirectory.RecurseSubdirectories;
                    enumerationOptions.MaxRecursionDepth = searchingDirectory.MaxRecursionDepth - 1;

                    IEnumerable<EntryPath> directories = _directoryIO.EnumerateDirectoryPaths(
                        searchingDirectory.Path, enumerationOptions);

                    foreach (EntryPath directory in directories)
                    {
                        yield return directory;
                    }
                }
            }
        }
    }

    /// <include file='Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="EnumerateDirectories[]"]/*' />
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

            EnumerationOptions enumerationOptions = GetDirectoriesEnumerationOptions();
            enumerationOptions.RecurseSubdirectories = directory.RecurseSubdirectories;
            enumerationOptions.MaxRecursionDepth = directory.MaxRecursionDepth;

            IEnumerable<EntryPath> directoryPaths = _directoryIO.EnumerateDirectoryPaths(
                directory.Path, enumerationOptions);

            foreach (EntryPath path in directoryPaths)
            {
                yield return path;
            }
        }
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private readonly List<SearchingFile> _includingFiles = [];
    private readonly List<SearchingFile> _excludingFiles = [];
    private IDirectoryIO _directoryIO = DirectoryIO.Default;
    private IErrorHandler? _errorHandler;
    private static EnumerationOptions GetDirectoriesEnumerationOptions() => new()
    {
        IgnoreInaccessible = true,
        MatchType = MatchType.Simple,
        AttributesToSkip = (FileAttributes)Int32.MinValue,
    };
    private static EnumerationOptions GetFilesEnumerationOptions() => new()
    {
        IgnoreInaccessible = true,
        MatchType = MatchType.Simple,
        AttributesToSkip = (FileAttributes)Int32.MinValue,
    };
    private EntryPathsProvider AddOptionHelper<T>(List<T> list, ref T item)
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
    private EntryPathsProvider()
    {

    }
    #endregion
}

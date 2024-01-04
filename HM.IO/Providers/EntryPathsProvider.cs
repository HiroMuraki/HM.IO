using System.Collections.Immutable;

namespace HM.IO.Providers;

/// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Class[@name="EntryPathsProvider"]/*' />
public sealed class EntryPathsProvider
    : IFilesProvider, IDirectoriesProvider
{
    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Static[@name="Create[]"]/*' />
    public static EntryPathsProvider Create()
    {
        return new EntryPathsProvider();
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="UseDirectoryIO[IDirectoryIO]"]/*' />
    public EntryPathsProvider UseDirectoryIO(IDirectoryIO directoryIO)
    {
        ArgumentNullException.ThrowIfNull(directoryIO, nameof(directoryIO));

        _directoryIO = directoryIO;

        return this;
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="UseErrorHandler[IErrorHandler]"]/*' />
    public EntryPathsProvider UseErrorHandler(IErrorHandler errorHandler)
    {
        ArgumentNullException.ThrowIfNull(errorHandler, nameof(errorHandler));

        _errorHandler = errorHandler;

        return this;
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="IncludeDirectory[SearchingDirectory]"]/*' />
    public EntryPathsProvider IncludeDirectory(SearchingDirectory entryPath)
    {
        return (EntryPathsProvider)AddOptionHelper(_includingDirectories, ref entryPath);
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="ExcludeDirectory[SearchingDirectory]"]/*' />
    public EntryPathsProvider ExcludeDirectory(SearchingDirectory entryPath)
    {
        return (EntryPathsProvider)AddOptionHelper(_excludingDirectories, ref entryPath);
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="IncludeFile[SearchingFile]"]/*' />
    public EntryPathsProvider IncludeFile(SearchingFile entryPath)
    {
        return (EntryPathsProvider)AddOptionHelper(_includingFiles, ref entryPath);
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="ExcludeFile[SearchingFile]"]/*' />
    public EntryPathsProvider ExcludeFile(SearchingFile entryPath)
    {
        return (EntryPathsProvider)AddOptionHelper(_excludingFiles, ref entryPath);
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="EnumerateFiles[]"]/*' />
    public IEnumerable<EntryPath> EnumerateFiles()
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
            foreach (EntryPath file in _directoryIO.EnumerateFiles(
                directory, GetFilesEnumerationOptions()))
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
                enumerationOptions.MaxRecursionDepth = searchingDirectory.MaxRecursionDepth;
                enumerationOptions.MaxRecursionDepth = searchingDirectory.MaxRecursionDepth - 1;

                IEnumerable<EntryPath> directories = _directoryIO.EnumerateDirectories(
                    searchingDirectory.Path, enumerationOptions);

                foreach (EntryPath directory in directories)
                {
                    yield return directory;
                }
            }
        }
    }

    /// <include file='Docs/Providers/EntryPathsProvider.xml' path='EntryPathsProvider/Methods/Instance[@name="EnumerateDirectories[]"]/*' />
    public IEnumerable<EntryPath> EnumerateDirectories()
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

            IEnumerable<EntryPath> directories = _directoryIO.EnumerateDirectories(
                directory.Path, enumerationOptions);

            foreach (EntryPath dir in directories)
            {
                yield return dir;
            }
        }
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private readonly List<SearchingFile> _includingFiles = [];
    private readonly List<SearchingFile> _excludingFiles = [];
    private IDirectoryIO _directoryIO = new DirectoryIO();
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

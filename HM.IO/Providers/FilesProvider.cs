﻿using System.Collections.Immutable;
using System.IO;

namespace HM.IO.Providers;

/// <summary>
/// Represents a sealed class for providing file-related operations and implements the <see cref="EntryPathsProvider"/> base class.
/// </summary>
public sealed class FilesProvider :
    IFilesProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="FilesProvider"/> with default directory input/output operations.
    /// </summary>
    /// <returns>A new <see cref="FilesProvider"/> instance.</returns>
    public static FilesProvider Create()
    {
        return new FilesProvider();
    }

    public FilesProvider UseDirectoryIO(IDirectoryIO directoryIO)
    {
        ArgumentNullException.ThrowIfNull(directoryIO, nameof(directoryIO));

        _directoryIO = directoryIO;

        return this;
    }

    public FilesProvider UseErrorHandler(IErrorHandler errorHandler)
    {
        ArgumentNullException.ThrowIfNull(errorHandler, nameof(errorHandler));

        _errorHandler = errorHandler;

        return this;
    }

    /// <summary>
    /// Includes a directory for processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the directory to be included.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider IncludeDirectory(SearchingDirectory entryPath)
    {
        return (FilesProvider)AddOptionHelper(_includingDirectories, ref entryPath);
    }

    /// <summary>
    /// Excludes a directory from processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the directory to be excluded.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider ExcludeDirectory(SearchingDirectory entryPath)
    {
        return (FilesProvider)AddOptionHelper(_excludingDirectories, ref entryPath);
    }

    /// <summary>
    /// Includes a file for processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the file to be included.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider IncludeFile(SearchingFile entryPath)
    {
        return (FilesProvider)AddOptionHelper(_includingFiles, ref entryPath);
    }

    /// <summary>
    /// Excludes a file from processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the file to be excluded.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider ExcludeFile(SearchingFile entryPath)
    {
        return (FilesProvider)AddOptionHelper(_excludingFiles, ref entryPath);
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
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
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateFiles();
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private readonly List<SearchingFile> _includingFiles = [];
    private readonly List<SearchingFile> _excludingFiles = [];
    private IDirectoryIO _directoryIO = new DirectoryIO();
    private IErrorHandler? _errorHandler;
    private IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory searchingDirectory)
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
    private FilesProvider AddOptionHelper<T>(List<T> list, ref T item)
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
    private FilesProvider()
    {

    }
    #endregion
}

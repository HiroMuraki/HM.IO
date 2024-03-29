﻿using System.Collections.Immutable;

namespace HM.IO.Providers;

public sealed class LocalFilePathsProvider :
    IFilePathsProvider
{
    public static LocalFilePathsProvider Create()
    {
        return new LocalFilePathsProvider();
    }

    public LocalFilePathsProvider Include(IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory searchingDirectory in searchingDirectories)
        {
            SearchingDirectory copy = searchingDirectory;
            AddOptionHelper(_includingDirectories, ref copy);
        }

        return this;
    }

    public LocalFilePathsProvider Exclude(IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory searchingDirectory in searchingDirectories)
        {
            SearchingDirectory copy = searchingDirectory;
            AddOptionHelper(_excludingDirectories, ref copy);
        }

        return this;
    }

    public LocalFilePathsProvider Include(IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile searchingDirectory in searchingFiles)
        {
            SearchingFile copy = searchingDirectory;
            AddOptionHelper(_includingFiles, ref copy);
        }

        return this;
    }

    public LocalFilePathsProvider Exclude(IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile searchingDirectory in searchingFiles)
        {
            SearchingFile copy = searchingDirectory;
            AddOptionHelper(_excludingFiles, ref copy);

        }

        return this;
    }

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
                foreach (EntryPath file in LocalDirectoryIO.EnumerateFilePaths(directory, GetFilesEnumerationOptions()))
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
                    if (!LocalDirectoryIO.Exists(searchingDirectory.Path))
                    {
                        if (searchingDirectory.IgnoreIfNotExists)
                        {
                            yield break;
                        }
                        else
                        {
                            throw new DirectoryNotFoundException(searchingDirectory.Path.StringPath);
                        }
                    }

                    EnumerationOptions enumerationOptions = GetDirectoriesEnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = searchingDirectory.RecurseSubdirectories;
                    enumerationOptions.MaxRecursionDepth = searchingDirectory.MaxRecursionDepth - 1;

                    IEnumerable<EntryPath> directories = LocalDirectoryIO.EnumerateDirectoryPaths(
                        searchingDirectory.Path, enumerationOptions);

                    foreach (EntryPath directory in directories)
                    {
                        yield return directory;
                    }
                }
            }
        }
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private readonly List<SearchingFile> _includingFiles = [];
    private readonly List<SearchingFile> _excludingFiles = [];
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
    private void AddOptionHelper<T>(List<T> list, ref T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }
    #endregion
}

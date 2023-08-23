﻿namespace HM.IO;

/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider
    : EntryPathProvider, IFilesProvider
{
    /// <summary>
    /// Gets the list of directory paths to include during enumeration.
    /// </summary>
    public List<String> IncludingDirectories { get; init; } = new();
    /// <summary>
    /// Gets the list of file paths to include during enumeration.
    /// </summary>
    public List<String> IncludingFiles { get; init; } = new();
    /// <summary>
    /// Gets the list of directory paths to exclude during enumeration.
    /// </summary>
    public List<String> ExcludingDirectories { get; init; } = new();
    /// <summary>
    /// Gets the list of file paths to exclude during enumeration.
    /// </summary>
    public List<String> ExcludingFiles { get; init; } = new();

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateFiles()
    {
        var includingFiles = SelectNotEmptyAsEntryPath(IncludingFiles);
        var excludingFiles = SelectNotEmptyAsEntryPath(ExcludingFiles);
        var enumeratioinOptons = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false,
            AttributesToSkip = FileAttributes.Normal,
        };
        var directoriesProvider = new DirectoriesProvider()
        {
            IncludingDirectories = IncludingDirectories,
            ExcludingDirectories = ExcludingDirectories
        };

        var selectedFiles = new HashSet<EntryPath>(includingFiles, EntryPathComparer);
        foreach (var directory in directoriesProvider.EnumerateDirectories())
        {
            foreach (var file in DirectoryIO.EnumerateFiles(directory, enumeratioinOptons))
            {
                selectedFiles.Add(file);
            }
        }
        selectedFiles.ExceptWith(excludingFiles);

        foreach (var file in selectedFiles.Order(EntryPathComparer))
        {
            yield return file;
        }
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateFiles();
    }
}

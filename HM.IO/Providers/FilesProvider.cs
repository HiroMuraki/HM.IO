using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace HM.IO.Providers;

/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider :
    EntryPathsProvider,
    IFilesProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="FilesProvider"/> with default directory input/output operations.
    /// </summary>
    /// <returns>A new <see cref="FilesProvider"/> instance.</returns>
    public static FilesProvider Create()
    {
        return Create(new DirectoryIO());
    }

    /// Creates a new instance of <see cref="FilesProvider"/> with custom directory input/output operations.
    /// </summary>
    /// <param name="directoryIO">Custom directory input/output operations implementation.</param>
    /// <returns>A new <see cref="FilesProvider"/> instance with the specified <paramref name="directoryIO"/> implementation.</returns>
    public static FilesProvider Create(IDirectoryIO directoryIO)
    {
        return new FilesProvider { DirectoryIO = directoryIO };
    }

    /// <summary>
    /// Includes a directory for processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the directory to be included.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider IncludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_includingDirectories, ref entryPath);
    }

    /// <summary>
    /// Excludes a directory from processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the directory to be excluded.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider ExcludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_excludingDirectories, ref entryPath);
    }

    /// <summary>
    /// Includes a file for processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the file to be included.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider IncludeFile(EntryPath entryPath)
    {
        return AddPathHelper(_includingFiles, ref entryPath);
    }

    /// <summary>
    /// Excludes a file from processing by the <see cref="FilesProvider"/>.
    /// </summary>
    /// <param name="entryPath">Path of the file to be excluded.</param>
    /// <returns>The updated <see cref="FilesProvider"/> instance.</returns>
    public FilesProvider ExcludeFile(EntryPath entryPath)
    {
        return AddPathHelper(_excludingFiles, ref entryPath);
    }

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateFiles()
    {
        // Yield from files
        var includingFiles = _includingFiles
             .Where(x => !String.IsNullOrEmpty(x.StringPath))
             .ToList();
        var excludingFiles = _excludingFiles
            .Where(x => !String.IsNullOrEmpty(x.StringPath))
            .ToImmutableHashSet();

        foreach (EntryPath file in includingFiles)
        {
            if (CanInclude(file))
            {
                yield return file;
            }
        }

        // yield from directories
        var directories = _includingDirectories
            .Except(_excludingDirectories)
            .ToList();

        var enumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false,
            AttributesToSkip = (FileAttributes)Int32.MinValue,
        };

        foreach (EntryPath directory in directories)
        {
            foreach (EntryPath file in DirectoryIO.EnumerateFiles(directory, enumerationOptions))
            {
                if (CanInclude(file))
                {
                    yield return file;
                }
            }
        }

        Boolean CanInclude(EntryPath entryPath)
        {
            return !excludingFiles.Any(e => e == entryPath);
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

    #region NonPublic
    private readonly List<EntryPath> _includingDirectories = [];
    private readonly List<EntryPath> _excludingDirectories = [];
    private readonly List<EntryPath> _includingFiles = [];
    private readonly List<EntryPath> _excludingFiles = [];
    private FilesProvider()
    {

    }
    private FilesProvider AddPathHelper(List<EntryPath> list, ref EntryPath entryPath)
    {
        if (!list.Contains(entryPath))
        {
            list.Add(entryPath);
        }

        return this;
    }
    #endregion
}

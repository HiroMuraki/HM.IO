using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace HM.IO.Providers;


/// <summary>
/// Provides file enumeration based on inclusion and exclusion filters.
/// </summary>
public sealed class FilesProvider :
    EntryPathProvider,
    IFilesProvider
{
    /// <summary>
    /// Gets the list of directory paths to include during enumeration.
    /// </summary>
    public List<SearchingDirectory> IncludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of directory paths to exclude during enumeration.
    /// </summary>
    public List<SearchingDirectory> ExcludingDirectories { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to include during enumeration.
    /// </summary>
    public List<SearchingFile> IncludingFiles { get; init; } = new();

    /// <summary>
    /// Gets the list of file paths to exclude during enumeration.
    /// </summary>
    public List<SearchingFile> ExcludingFiles { get; init; } = new();

    /// <summary>
    /// Enumerates files based on the provided inclusion and exclusion filters.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="EntryPath"/> instances representing files.</returns>
    public IEnumerable<EntryPath> EnumerateFiles()
    {
        IEnumerable<EntryPath> includingFiles = IncludingFiles
            .Where(x => !String.IsNullOrEmpty(x.FilePath.StringPath))
            .Select(x => x.FilePath)
            .Distinct();
        var excludingFiles = ExcludingFiles
            .Where(x => !String.IsNullOrEmpty(x.FilePath.StringPath))
            .Select(x => x.FilePath)
            .ToImmutableHashSet();

        foreach (EntryPath file in includingFiles)
        {
            if (CanInclude(file))
            {
                yield return file;
            }
        }

        var enumerationOptions = new EnumerationOptions()
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = false,
            AttributesToSkip = (FileAttributes)Int32.MinValue,
        };

        IEnumerable<EntryPath> directories = new DirectoriesProvider()
        {
            IncludingDirectories = IncludingDirectories,
            ExcludingDirectories = ExcludingDirectories
        }.EnumerateDirectories();

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

        Boolean CanInclude(EntryPath path)
        {
            return !excludingFiles.Any(e => e == path);
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

public class FilesProviderX :
    EntryPathProvider,
    IFilesProvider
{
    public static FilesProviderX Create()
    {
        return new FilesProviderX();
    }

    public FilesProviderX IncludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_includingDirectories, ref entryPath);
    }

    public FilesProviderX ExcludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_excludingDirectories, ref entryPath);
    }

    public FilesProviderX IncludeFile(EntryPath entryPath)
    {
        return AddPathHelper(_includingFiles, ref entryPath);
    }

    public FilesProviderX ExcludeFile(EntryPath entryPath)
    {
        return AddPathHelper(_excludingFiles, ref entryPath);
    }

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

    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateFiles();
    }

    #region NonPublic
    private readonly List<EntryPath> _includingDirectories = [];
    private readonly List<EntryPath> _excludingDirectories = [];
    private readonly List<EntryPath> _includingFiles = [];
    private readonly List<EntryPath> _excludingFiles = [];
    private FilesProviderX()
    {

    }
    private FilesProviderX AddPathHelper(List<EntryPath> list, ref EntryPath entryPath)
    {
        if (!list.Contains(entryPath))
        {
            list.Add(entryPath);
        }

        return this;
    }
    #endregion
}

public static class FilesProviderExtensions
{
    #region IncludeDirectory
    public static FilesProviderX IncludeDirectory(this FilesProviderX self, String path)
    {
        return self.IncludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProviderX IncludeDirectories(this FilesProviderX self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeDirectory(item);
        }

        return self;
    }

    public static FilesProviderX IncludeDirectories(this FilesProviderX self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    public static FilesProviderX IncludeFile(this FilesProviderX self, String path)
    {
        return self.IncludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProviderX IncludeFiles(this FilesProviderX self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeFile(item);
        }

        return self;
    }

    public static FilesProviderX IncludeFiles(this FilesProviderX self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static FilesProviderX ExcludeDirectory(this FilesProviderX self, String path)
    {
        return self.ExcludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProviderX ExcludeDirectories(this FilesProviderX self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeDirectory(item);
        }

        return self;
    }

    public static FilesProviderX ExcludeDirectories(this FilesProviderX self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static FilesProviderX ExcludeFile(this FilesProviderX self, String path)
    {
        return self.ExcludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProviderX ExcludeFiles(this FilesProviderX self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeFile(item);
        }

        return self;
    }

    public static FilesProviderX ExcludeFiles(this FilesProviderX self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}

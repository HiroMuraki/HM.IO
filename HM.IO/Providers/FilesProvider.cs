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
    public static FilesProvider Create()
    {
        return Create(new DirectoryIO());
    }

    public static FilesProvider Create(IDirectoryIO directoryIO)
    {
        return new FilesProvider { DirectoryIO = directoryIO };
    }

    public FilesProvider IncludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_includingDirectories, ref entryPath);
    }

    public FilesProvider ExcludeDirectory(EntryPath entryPath)
    {
        return AddPathHelper(_excludingDirectories, ref entryPath);
    }

    public FilesProvider IncludeFile(EntryPath entryPath)
    {
        return AddPathHelper(_includingFiles, ref entryPath);
    }

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

public static class FilesProviderExtensions
{
    #region IncludeDirectory
    public static FilesProvider IncludeDirectory(this FilesProvider self, String path)
    {
        return self.IncludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeDirectory(item);
        }

        return self;
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    public static FilesProvider IncludeFile(this FilesProvider self, String path)
    {
        return self.IncludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeFile(item);
        }

        return self;
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static FilesProvider ExcludeDirectory(this FilesProvider self, String path)
    {
        return self.ExcludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeDirectory(item);
        }

        return self;
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static FilesProvider ExcludeFile(this FilesProvider self, String path)
    {
        return self.ExcludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeFile(item);
        }

        return self;
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}

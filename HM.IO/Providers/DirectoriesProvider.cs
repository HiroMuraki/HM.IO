using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HM.IO.Providers;

public sealed class DirectoriesProvider :
    EntryPathsProvider,
    IDirectoriesProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="DirectoriesProvider"/> with default directory input/output operations.
    /// </summary>
    /// <returns>A new <see cref="DirectoriesProvider"/> instance.</returns>
    public static DirectoriesProvider Create()
    {
        return Create(new DirectoryIO());
    }

    /// <summary>
    /// Creates a new instance of <see cref="DirectoriesProvider"/> with custom directory input/output operations.
    /// </summary>
    /// <param name="directoryIO">Custom directory input/output operations implementation.</param>
    /// <returns>A new <see cref="DirectoriesProvider"/> instance with the specified <paramref name="directoryIO"/> implementation.</returns>
    public static DirectoriesProvider Create(IDirectoryIO directoryIO)
    {
        return new DirectoriesProvider() { DirectoryIO = directoryIO };
    }

    /// <summary>
    /// Includes a directory for processing by the <see cref="DirectoriesProvider"/>.
    /// </summary>
    /// <param name="path">Path of the directory to be included.</param>
    /// <returns>The updated <see cref="DirectoriesProvider"/> instance.</returns>
    public DirectoriesProvider IncludeDirectory(SearchingDirectory path)
    {
        return (DirectoriesProvider)AddOptionHelper(_includingDirectories, ref path);
    }

    /// <summary>
    /// Excludes a directory from processing by the <see cref="DirectoriesProvider"/>.
    /// </summary>
    /// <param name="path">Path of the directory to be excluded.</param>
    /// <returns>The updated <see cref="DirectoriesProvider"/> instance.</returns>
    public DirectoriesProvider ExcludeDirectory(SearchingDirectory path)
    {
        return (DirectoriesProvider)AddOptionHelper(_excludingDirectories, ref path);
    }

    /// <summary>
    /// Enumerates and returns a collection of directories processed by the <see cref="DirectoriesProvider"/>.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of directory paths.</returns>
    public IEnumerable<EntryPath> EnumerateDirectories()
    {
        var excludedDirectories = _excludingDirectories
            .SelectMany(EnumerateDirectories)
            .ToImmutableHashSet();

        return _includingDirectories
            .SelectMany(EnumerateDirectories)
            .SkipWhile(excludedDirectories.Contains);
    }

    /// <summary>
    /// Overrides the base class method to enumerate and return a collection of directories processed by the <see cref="DirectoriesProvider"/>.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of directory paths.</returns>
    public override IEnumerable<EntryPath> EnumerateItems()
    {
        return EnumerateDirectories();
    }

    #region NonPublic
    private readonly List<SearchingDirectory> _includingDirectories = [];
    private readonly List<SearchingDirectory> _excludingDirectories = [];
    private IEnumerable<EntryPath> EnumerateDirectories(SearchingDirectory directory)
    {
        if (!DirectoryIO.Exists(directory.Path))
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

        IEnumerable<EntryPath> directories = DirectoryIO.EnumerateDirectories(directory.Path, new EnumerationOptions
        {
            IgnoreInaccessible = true,
            RecurseSubdirectories = directory.RecurseSubdirectories,
            MaxRecursionDepth = directory.MaxRecursionDepth,
            AttributesToSkip = (FileAttributes)Int32.MinValue,
        });

        foreach (EntryPath dir in directories)
        {
            yield return dir;
        }
    }
    private DirectoriesProvider()
    {

    }
    #endregion
}

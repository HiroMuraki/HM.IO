using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace HM.IO.Providers;

/// <summary>
/// Represents a sealed class for providing directory-related operations and implements the <see cref="EntryPathsProvider"/> base class.
/// </summary>
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
        return new DirectoriesProvider();
    }

    public DirectoriesProvider UseDirectoryIO(IDirectoryIO directoryIO)
    {
        return UseDirectoryIO<DirectoriesProvider>(directoryIO);
    }

    public DirectoriesProvider UseErrorHandler(IErrorHandler errorHandler)
    {
        return UseErrorHandler<DirectoriesProvider>(errorHandler);
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
    private DirectoryEnumerationOptions _directoriesEnumerationOptions = new()
    {
        IgnoreInaccessible = true,
        AttributesToSkip = (FileAttributes)Int32.MinValue,
    };
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

        var enumerationOptions = _directoriesEnumerationOptions.ToEnumerationOptions();
        enumerationOptions.RecurseSubdirectories = directory.RecurseSubdirectories;
        enumerationOptions.MaxRecursionDepth = directory.MaxRecursionDepth;

        IEnumerable<EntryPath> directories = DirectoryIO.EnumerateDirectories(
            directory.Path, enumerationOptions);

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

using HM.Common;
using System.Diagnostics.CodeAnalysis;
using BclDirectory = System.IO.Directory;

namespace HM.IO.Previews;

public sealed class LocalDirectory :
    LocalEntryBase,
    IDirectoryEntry,
    IEquatable<LocalDirectory>
{
    public DirectoryPath Path => _path;

    public override Boolean Exists => BclDirectory.Exists(Path.StringPath);

    public IEnumerable<LocalDirectory> EnumerateLocalDirectories(EnumerationOptions enumerationOptions)
    {
        foreach (String path in BclDirectory.EnumerateDirectories(Path.StringPath, "*", enumerationOptions))
        {
            yield return new LocalDirectory(new DirectoryPath(path));
        }
    }

    public IEnumerable<IDirectoryEntry> EnumerateDirectory(EnumerationOptions enumerationOptions)
       => EnumerateLocalDirectories(enumerationOptions);

    public IEnumerable<LocalFile> EnumerateLocalFiles(EnumerationOptions enumerationOptions)
    {
        foreach (String path in BclDirectory.EnumerateFiles(Path.StringPath, "*", enumerationOptions))
        {
            yield return new LocalFile(new FilePath(path));
        }
    }

    public IEnumerable<IFileEntry> EnumerateFiles(EnumerationOptions enumerationOptions)
        => EnumerateLocalFiles(enumerationOptions);

    public Boolean Equals(LocalDirectory? other)
        => ComparisonHelper.ClassIEquatableEquals(this, other, LocalDirectoryEqualityComparer.Instance);

    public override Boolean Equals(Object? obj)
        => ComparisonHelper.ClassEquals(this, obj);

    public override Int32 GetHashCode()
        => LocalDirectoryEqualityComparer.Instance.GetHashCode(this);

    public LocalDirectory(String path) : this(new DirectoryPath(path))
    {

    }

    public LocalDirectory(DirectoryPath path)
    {
        _path = path;
    }

    #region NonPublic
    protected override String GetStringPath() => Path.StringPath;
    private readonly DirectoryPath _path;
    private class LocalDirectoryEqualityComparer : EqualityComparer<LocalDirectory>
    {
        public static LocalDirectoryEqualityComparer Instance { get; } = new();

        public override Boolean Equals(LocalDirectory? x, LocalDirectory? y)
        {
            return x!.Path == y!.Path;
        }

        public override Int32 GetHashCode([DisallowNull] LocalDirectory obj)
        {
            return obj.Path.GetHashCode();
        }
    }
    #endregion
}

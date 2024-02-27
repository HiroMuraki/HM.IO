﻿using HM.CommonComponent;
using HM.IO.Previews;
using System.Diagnostics.CodeAnalysis;

namespace PreviewModuleTestGround.Preview;

public sealed class LocalDirectory :
    LocalEntryBase,
    IDirectory,
    IEquatable<LocalDirectory>
{
    public DirectoryPath Path => _path;

    public override Boolean Exists => Directory.Exists(Path.StringPath);

    public IEnumerable<LocalDirectory> EnumerateLocalDirectories(EnumerationOptions enumerationOptions)
    {
        foreach (String path in Directory.EnumerateDirectories(Path.StringPath, "*", enumerationOptions))
        {
            yield return new LocalDirectory(new DirectoryPath(path));
        }
    }

    public IEnumerable<IDirectory> EnumerateDirectory(EnumerationOptions enumerationOptions)
       => EnumerateLocalDirectories(enumerationOptions);

    public IEnumerable<LocalFile> EnumerateLocalFiles(EnumerationOptions enumerationOptions)
    {
        foreach (String path in Directory.EnumerateFiles(Path.StringPath, "*", enumerationOptions))
        {
            yield return new LocalFile(new FilePath(path));
        }
    }

    public IEnumerable<IFile> EnumerateFiles(EnumerationOptions enumerationOptions)
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

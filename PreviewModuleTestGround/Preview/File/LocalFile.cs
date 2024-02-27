﻿using HM.CommonComponent;
using HM.IO.Previews;
using System.Diagnostics.CodeAnalysis;

namespace PreviewModuleTestGround.Preview;

public sealed class LocalFile :
    LocalEntryBase,
    IFile,
    IEquatable<LocalFile>
{
    public FilePath Path => _filePath;

    public override Boolean Exists => File.Exists(Path.StringPath);

    public Int64 SizeInBytes => new FileInfo(Path.StringPath).Length;

    public void Create()
    {
        File.Create(Path.StringPath).Dispose();
    }

    public void Delete()
    {
        File.Delete(Path.StringPath);
    }

    public IStream Open(StreamMode mode)
    {
        return new LocalFileStream(Path, mode);
    }

    public Boolean Equals(LocalFile? other)
        => ComparisonHelper.ClassIEquatableEquals(this, other, LocalFileEqualityComparer.Instance);

    public override Boolean Equals(Object? obj)
        => ComparisonHelper.ClassEquals(this, obj);

    public override Int32 GetHashCode() 
        => LocalFileEqualityComparer.Instance.GetHashCode(this);

    public LocalFile(String path) : this(new FilePath(path))
    {

    }

    public LocalFile(FilePath filePath)
    {
        _filePath = filePath;
    }

    #region NonPublic
    protected override String GetStringPath() => Path.StringPath;
    private readonly FilePath _filePath;
    private class LocalFileEqualityComparer : EqualityComparer<LocalFile>
    {
        public static LocalFileEqualityComparer Instance { get; } = new();

        public override Boolean Equals(LocalFile? x, LocalFile? y)
        {
            return x!.Path == y!.Path;
        }

        public override Int32 GetHashCode([DisallowNull] LocalFile obj)
        {
            return obj.Path.GetHashCode();
        }
    }
    #endregion
}

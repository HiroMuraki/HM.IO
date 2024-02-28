using HM.Common;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Previews;

public readonly struct DirectoryPath :
    IEntryPath<DirectoryPath>
{
    public static DirectoryPath Empty { get; }

    public readonly String StringPath => _filePath;

    public readonly DirectoryPath ParentDirectory => new(Path.GetDirectoryName(StringPath) ?? String.Empty);

    public readonly String DirectoryName => Path.GetFileNameWithoutExtension(StringPath);

    public readonly Boolean Equals(DirectoryPath other)
    {
        return StringPath == other.StringPath;
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly Int32 CompareTo(DirectoryPath other)
    {
        return StringPath.CompareTo(other.StringPath);
    }

    public readonly Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public readonly override Int32 GetHashCode()
    {
        return StringPath.GetHashCode();
    }

    public static Boolean operator ==(DirectoryPath left, DirectoryPath right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(DirectoryPath left, DirectoryPath right)
    {
        return !(left == right);
    }

    public DirectoryPath()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(DirectoryPath));
        _filePath = String.Empty;
    }

    public DirectoryPath(String stringPath)
    {
        _filePath = stringPath;
    }

    #region NonPublic
    private readonly String _filePath;
    #endregion
}

using HM.Common;
using HM.IO.Previews.Directory;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Previews.File;

public readonly struct FilePath :
    IEntryPath<FilePath>
{
    public static FilePath Empty { get; }

    public readonly String StringPath => _filePath;

    public readonly DirectoryPath ParentDirectory => new(Path.GetDirectoryName(StringPath) ?? String.Empty);

    public readonly String FileName => Path.GetFileNameWithoutExtension(StringPath);

    public readonly String FileExtension => Path.GetExtension(StringPath);

    public readonly Boolean Equals(FilePath other)
    {
        return StringPath == other.StringPath;
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly Int32 CompareTo(FilePath other)
    {
        return StringPath.CompareTo(other.StringPath);
    }

    public readonly Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public readonly override Int32 GetHashCode()
    {
        return StringPath.GetHashCode();
    }

    public static Boolean operator ==(FilePath left, FilePath right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(FilePath left, FilePath right)
    {
        return !(left == right);
    }

    public override String ToString()
    {
        return StringPath;
    }

    public FilePath()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(FilePath));
        _filePath = String.Empty;
    }

    public FilePath(String stringPath)
    {
        ArgumentNullException.ThrowIfNull(stringPath);

        _filePath = stringPath;
    }

    #region NonPublic
    private readonly String _filePath;
    #endregion
}

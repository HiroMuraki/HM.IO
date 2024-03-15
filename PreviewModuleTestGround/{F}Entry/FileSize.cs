using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.IO.Previews;

public readonly struct FileSize :
    IEquatable<FileSize>,
    IEqualityOperators<FileSize, FileSize, Boolean>,
    IComparable<FileSize>,
    IComparable
{
    public readonly Int64 SizeInBytes => _sizeInBytes;

    public readonly Double SizeInKBytes => SizeInBytes / 1024.0;

    public readonly Double SizeInMBytes => SizeInKBytes / 1024.0;

    public readonly Double SizeInGBytes => SizeInMBytes / 1024.0;

    public readonly Boolean Equals(FileSize other)
    {
        return _sizeInBytes == other._sizeInBytes;
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly Int32 CompareTo(FileSize other)
    {
        return _sizeInBytes.CompareTo(other._sizeInBytes);
    }

    public readonly Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public readonly override Int32 GetHashCode()
    {
        return _sizeInBytes.GetHashCode();
    }

    public static Boolean operator ==(FileSize left, FileSize right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(FileSize left, FileSize right)
    {
        return !(left == right);
    }

    public readonly override String ToString()
        => ToStringHelper.Build(this);

    public static FileSize FromBytes(Int64 bytes)
    {
        return new FileSize(bytes);
    }

    public static FileSize FromKBytes(Int64 kBytes)
    {
        return FromBytes(kBytes * 1024);
    }

    public static FileSize FromMBytes(Int64 mBytes)
    {
        return FromKBytes(mBytes * 1024);
    }

    public static FileSize FromGBytes(Int64 gBytes)
    {
        return FromMBytes(gBytes * 1024);
    }

    public FileSize()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(FileSize));
    }

    #region NonPublic
    private readonly Int64 _sizeInBytes;
    private FileSize(Int64 size)
    {
        _sizeInBytes = size;
    }
    #endregion
}
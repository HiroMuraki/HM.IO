using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.Environment;

public readonly struct MyDirectory :
    IEquatable<MyDirectory>,
    IComparable<MyDirectory>,
    IEqualityOperators<MyDirectory, MyDirectory, Boolean>,
    IComparable
{
    public MyDirectory()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(MyDirectory));
        Path = String.Empty;
    }

    public MyDirectory(String path)
    {
        Path = path;
    }

    public String Path { get; }

    public readonly MyDirectory GetPath(String pathName)
    {
        return new MyDirectory(System.IO.Path.Combine(Path, pathName));
    }

    public Boolean Equals(MyDirectory other)
    {
        return Path == other.Path;
    }

    public override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public Int32 CompareTo(MyDirectory other)
    {
        return Path.CompareTo(other.Path);
    }

    public Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public override Int32 GetHashCode()
    {
        return Path.GetHashCode();
    }

    public static Boolean operator ==(MyDirectory left, MyDirectory right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(MyDirectory left, MyDirectory right)
    {
        return !(left == right);
    }
}

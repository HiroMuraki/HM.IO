using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.AppComponents;

public readonly struct AppPath :
    IEquatable<AppPath>,
    IComparable<AppPath>,
    IComparable,
    IEqualityOperators<AppPath, AppPath, Boolean>
{
    public AppPath(String path, AppPathType pathType)
    {
        Path = path;
        PathType = pathType;
    }

    public AppPath(String[] paths, AppPathType pathType)
    {
        Path = System.IO.Path.Combine(paths);
        PathType = pathType;
    }

    public String Path { get; } = String.Empty;

    public AppPathType PathType { get; }

    public String GetSubPath(String path)
    {
        return System.IO.Path.Combine(Path, path);
    }

    public void EnsureCreated()
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
    }

    public IEnumerable<String> EnumerateFiles()
        => EnumerateFiles(new EnumerationOptions());

    public IEnumerable<String> EnumerateFiles(EnumerationOptions enumerationOptions)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
            yield break;
        }

        foreach (String file in Directory.EnumerateFiles(Path, "*", enumerationOptions))
        {
            yield return file;
        }
    }

    public Boolean Equals(AppPath other)
        => Path == other.Path;

    public override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public override Int32 GetHashCode()
        => Path.GetHashCode();

    public Int32 CompareTo(AppPath other)
        => ComparisonHelper.StructCompareTo(this, other);

    public Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public static Boolean operator ==(AppPath left, AppPath right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(AppPath left, AppPath right)
    {
        return !(left == right);
    }
}


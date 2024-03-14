using HM.Common;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.IO;

public readonly struct EntryPath :
    IEquatable<EntryPath>,
    IEqualityOperators<EntryPath, EntryPath, Boolean>,
    IComparable<EntryPath>,
    IComparable
{
    #region Properties
    public static EntryPath Empty { get; } = new EntryPath(String.Empty);

    public readonly String this[Int32 index]
    {
        get
        {
            return GetRoutes()[index];
        }
    }

    public readonly String this[Index index]
    {
        get
        {
            return GetRoutes()[index];
        }
    }

    public readonly EntryPath this[Range range]
    {
        get
        {
            return new EntryPath(String.Join(SystemPathSeparatorChar, GetRoutes()[range]));
        }
    }

    public readonly String[] Routes => GetRoutes();

    public readonly String StringPath
    {
        get
        {
            return _stringPath;
        }
    }

    public readonly EntryPath FullPath
    {
        get
        {
            return Create(Path.GetFullPath(StringPath));
        }
    }

    public readonly String? PathRoot
    {
        get
        {
            return Path.GetPathRoot(StringPath);
        }
    }

    public readonly EntryPath DirectoryName
    {
        get
        {
            String[] routes = GetRoutes();
            return new EntryPath(String.Join(SystemPathSeparatorChar, routes[0..(routes.Length - 1)]));
        }
    }

    public readonly String EntryName
    {
        get
        {
            return GetRoutes()[^1];
        }
    }

    public readonly Int32 LengthOfRoutes
    {
        get
        {
            return GetRoutes().Length;
        }
    }
    #endregion

    #region Methods
    public readonly Boolean IsSubPathOf(EntryPath otherPath)
    {
        if (StringPath.Length <= otherPath.StringPath.Length)
        {
            return false;
        }

        String[] routesOfThis = GetRoutes();
        String[] routesOfOther = otherPath.GetRoutes();
        for (Int32 i = 0; i < otherPath.LengthOfRoutes; i++)
        {
            if (routesOfThis[i] != routesOfOther[i])
            {
                return false;
            }
        }

        return true;
    }

    public readonly Boolean IsParentPathOf(EntryPath otherPath)
    {
        return otherPath.IsSubPathOf(this);
    }

    public readonly Boolean Equals(EntryPath other)
    {
        return StringPath == other.StringPath;
    }

    public readonly Int32 CompareTo(EntryPath other)
    {
        return StringPath.CompareTo(other.StringPath);
    }

    public readonly Int32 CompareTo(Object? obj)
        => ComparisonHelper.StructCompareTo(this, obj);

    public readonly override String ToString()
    {
        return StringPath;
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
        => ComparisonHelper.StructEquals(this, obj);

    public readonly override Int32 GetHashCode()
    {
        unchecked
        {
            String[] routes = GetRoutes();
            Int32 hashCode = routes.Length ^ 17;

            if (routes.Length > 0)
            {
                hashCode = HashCode.Combine(hashCode, routes[0].ToUpper()) * 31;
            }
            if (routes.Length > 1)
            {
                hashCode = HashCode.Combine(hashCode, routes[^1].ToUpper()) * 31;
            }

            return hashCode;
        }
    }

    public static EntryPath Create(String stringPath)
    {
        if (String.IsNullOrWhiteSpace(stringPath))
        {
            throw new ArgumentException($"{nameof(stringPath)} can't be null, empty or white space-only");
        }

        String normalizedPath = Path.TrimEndingDirectorySeparator(stringPath);

        if (stringPath.Contains(AltPathSeparatorChar))
        {
            normalizedPath = normalizedPath.Replace(AltPathSeparatorChar, SystemPathSeparatorChar);
        }

        return new EntryPath(normalizedPath);
    }

    public static EntryPath Combine(EntryPath path1, EntryPath path2)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath));
    }

    public static EntryPath Combine(EntryPath path1, EntryPath path2, EntryPath path3)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath, path3.StringPath));
    }

    public static EntryPath Combine(EntryPath path1, EntryPath path2, EntryPath path3, EntryPath path4)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath, path3.StringPath, path4.StringPath));
    }

    public static EntryPath Combine(IEnumerable<EntryPath> paths)
    {
        return Create(Path.Combine(paths.Select(x => x.StringPath).ToArray()));
    }

    public static Boolean operator ==(EntryPath left, EntryPath right)
    {
        String[] leftRoutes = left.GetRoutes();
        String[] rightRoutes = right.GetRoutes();

        if (leftRoutes.Length != rightRoutes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < leftRoutes.Length; i++)
        {
            if (leftRoutes[i] != rightRoutes[i])
            {
                return false;
            }
        }

        return true;
    }

    public static Boolean operator !=(EntryPath left, EntryPath right)
    {
        return !(left == right);
    }

    public EntryPath()
    {
        ThrowHelper.ThrowUnableToCallDefaultConstructor(typeof(EntryPath));
        _stringPath = String.Empty;
    }
    #endregion

    #region NonPublic
    private static Char[] PathSeparatorChars => [SystemPathSeparatorChar, AltPathSeparatorChar];
    private static Char SystemPathSeparatorChar => Path.DirectorySeparatorChar;
    private static Char AltPathSeparatorChar => Path.AltDirectorySeparatorChar;
    private readonly String _stringPath;
    private EntryPath(String stringPath)
    {
        _stringPath = stringPath;
    }
    private String[] GetRoutes()
    {
        return StringPath.Split(PathSeparatorChars);
    }
    #endregion
}

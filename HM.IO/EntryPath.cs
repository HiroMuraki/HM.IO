#define PLATFORM_WINDOWS
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <include file='EntryPath.xml' path='EntryPath/Class[@name="EntryPath"]/*' />
public readonly struct EntryPath :
    IComparable<EntryPath>,
    IEquatable<EntryPath>
{
    #region Properties
    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Int32]"]/*' />
    public readonly String this[Int32 index] => GetRoutes()[index];

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Index]"]/*' />
    public readonly String this[Index index] => GetRoutes()[index];

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Range]"]/*' />
    public readonly EntryPath this[Range range] => new(GetRoutes()[range]);

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="Length"]/*' />
    public readonly Int32 Length => GetRoutes().Length;

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="StringPath"]/*' />
    public readonly String StringPath => _stringPath;

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="DirectoryName"]/*' />
    public EntryPath DirectoryName => new(GetRoutes()[0..(GetRoutes().Length - 1)]);

    /// <include file='EntryPath.xml' path='EntryPath/Properties/Instance[@name="EntryName"]/*' />
    public EntryPath EntryName => new(GetRoutes()[^1]);
    #endregion

    #region Methods
    /// <include file='EntryPath.xml' path='EntryPath/Methods/Instance[@name="IsSubPathOf[EntryPath]"]/*' />
    public Boolean IsSubPathOf(EntryPath otherPath)
    {
        if (_stringPath.Length <= otherPath._stringPath.Length)
        {
            return false;
        }

        String[] routesOfThis = GetRoutes();
        String[] routesOfOther = otherPath.GetRoutes();
        for (Int32 i = 0; i < otherPath.Length; i++)
        {
            if (routesOfThis[i] != routesOfOther[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <include file='EntryPath.xml' path='EntryPath/Methods/Instance[@name="IsParentPathOf[EntryPath]"]/*' />
    public Boolean IsParentPathOf(EntryPath otherPath)
    {
        return otherPath.IsSubPathOf(this);
    }

    public readonly override String ToString()
    {
        return String.Join("", GetRoutes().Select(p => $"[{p}]"));
    }

    public readonly override Boolean Equals([NotNullWhen(true)] Object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (GetType() != obj.GetType())
        {
            return false;
        }

        return Equals((EntryPath)obj);
    }

    public readonly override Int32 GetHashCode()
    {
        unchecked
        {
            String[] routes = GetRoutes();
            Int32 hashCode = routes.Length ^ 17;
            if (routes.Length > 0)
            {
                hashCode = HashCode.Combine(hashCode, routes[0]) * 31;
            }
            if (routes.Length > 1)
            {
                hashCode = HashCode.Combine(hashCode, routes[^1]) * 31;
            }
            return hashCode;
        }
    }

    public readonly Boolean Equals(EntryPath other)
    {
        return this == other;
    }

    public readonly Int32 CompareTo(EntryPath other)
    {
        String[] routesOfThis = GetRoutes();
        String[] routesOfOther = other.GetRoutes();

        Int32 minLength = routesOfThis.Length < routesOfOther.Length ? routesOfThis.Length : routesOfOther.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = String.Compare(routesOfThis[i], routesOfOther[i], StringComparison.Ordinal);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (routesOfThis.Length < routesOfOther.Length)
        {
            return -1;
        }
        else if (routesOfThis.Length > routesOfOther.Length)
        {
            return 1;
        }

        return 0;
    }

    /// <include file='EntryPath.xml' path='EntryPath/Methods/Class[@name="Create[String]"]/*' />
    public static EntryPath Create(String stringPath)
    {
        if (String.IsNullOrWhiteSpace(stringPath))
        {
            throw new ArgumentException($"{nameof(stringPath)} can't be null, empty or white space-only");
        }

        String normalizedPath = Path.TrimEndingDirectorySeparator(stringPath);
        if (stringPath.Contains(s_pathSeparatorChar[1]))
        {
            String[] routes = normalizedPath.Split(s_pathSeparatorChar);
            return new EntryPath(routes);
        }
        else
        {
            return new EntryPath(normalizedPath);
        }
    }

    public static Boolean operator ==(EntryPath left, EntryPath right)
    {
        String[] routesOfLeft = left.GetRoutes();
        String[] routesOfRight = right.GetRoutes();

        if (routesOfLeft.Length != routesOfRight.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < routesOfLeft.Length; i++)
        {
            if (routesOfLeft[i] != routesOfRight[i])
            {
                return false;
            }
        }

        return true;
    }

    public static Boolean operator !=(EntryPath left, EntryPath right)
    {
        return left == right;
    }

    public EntryPath()
    {
        throw new InvalidOperationException($"Unable to call default constructor of `{typeof(EntryPath)}`.");
    }
    #endregion

    #region NonPublic
    private readonly String _stringPath;
    private static readonly Char[] s_pathSeparatorChar =
    {
        Path.DirectorySeparatorChar,
        Path.AltDirectorySeparatorChar
    };
    private EntryPath(String stringPath)
    {
        _stringPath = stringPath;
    }
    private EntryPath(IEnumerable<String> routes)
    {
        _stringPath = String.Join(s_pathSeparatorChar[0], routes);
    }
    private String[] GetRoutes()
    {
        return _stringPath.Split(s_pathSeparatorChar);
    }
    #endregion
}

#define PLATFORM_WINDOWS
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <include file='Docs/EntryPath.xml' path='EntryPath/Class[@name="EntryPath"]/*' />
public readonly struct EntryPath :
    IComparable<EntryPath>,
    IEquatable<EntryPath>
{
    #region Properties
    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Int32]"]/*' />
    public readonly String this[Int32 index] => _routes[index];

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Index]"]/*' />
    public readonly String this[Index index] => _routes[index];

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="Indexer[Range]"]/*' />
    public readonly EntryPath this[Range range] => new(_routes[range]);

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="Length"]/*' />
    public readonly Int32 Length => _routes.Length;

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="StringPath"]/*' />
    public readonly String StringPath => Path.Combine(_routes);

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="DirectoryName"]/*' />
    public EntryPath DirectoryName => new(_routes[0..(_routes.Length - 1)]);

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Properties/Instance[@name="EntryName"]/*' />
    public EntryPath EntryName => new(_routes[^1]);
    #endregion

    #region Methods
    /// <include file='Docs/EntryPath.xml' path='EntryPath/Methods/Method[@name="IsSubPathOf[EntryPath]"]/*' />
    public Boolean IsSubPathOf(EntryPath otherPath)
    {
        if (_routes.Length <= otherPath._routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < otherPath.Length; i++)
        {
            if (_routes[i] != otherPath[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Methods/Method[@name="IsParentPathOf[EntryPath]"]/*' />
    public Boolean IsParentPathOf(EntryPath otherPath)
    {
        return otherPath.IsParentPathOf(this);
    }

    public readonly override String ToString()
    {
        return String.Join("", _routes.Select(p => $"[{p}]"));
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
            Int32 hashCode = _routes.Length ^ 17;
            if (_routes.Length > 0)
            {
                hashCode = HashCode.Combine(hashCode, _routes[0]) * 31;
            }
            if (_routes.Length > 1)
            {
                hashCode = HashCode.Combine(hashCode, _routes[^1]) * 31;
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
        Int32 minLength = _routes.Length < other._routes.Length ? _routes.Length : other._routes.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = String.Compare(_routes[i], other._routes[i], StringComparison.Ordinal);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (_routes.Length < other._routes.Length)
        {
            return -1;
        }
        else if (_routes.Length > other._routes.Length)
        {
            return 1;
        }

        return 0;
    }

    public static Boolean operator ==(EntryPath left, EntryPath right)
    {
        if (left!._routes.Length != right!._routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < left._routes.Length; i++)
        {
            if (left._routes[i] != right._routes[i])
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

    /// <include file='Docs/EntryPath.xml' path='EntryPath/Methods/Method[@name="CreateFromPath[String]"]/*' />
    public static EntryPath CreateFromPath(String path)
    {
        if (String.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"{nameof(path)} can't be null, empty or white space-only");
        }

        String normalizedPath = Path.TrimEndingDirectorySeparator(path);
        String[] routes = normalizedPath.Split(s_pathSeparatorChar);
        return new EntryPath(routes);
    }
    #endregion

    #region NonPublic
    private readonly String[] _routes;
    private static readonly Char[] s_pathSeparatorChar =
    {
        Path.DirectorySeparatorChar,
        Path.AltDirectorySeparatorChar
    };
    private EntryPath(params String[] routes)
    {
        _routes = routes;
    }
    #endregion
}

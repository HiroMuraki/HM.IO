using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace HM.IO;

/// <summary>
/// Represents a path to a directory or file.
/// </summary>
public readonly struct EntryPath :
    IComparable<EntryPath>,
    IEquatable<EntryPath>
{
    /// <summary>
    /// Gets the route at the specified index.
    /// </summary>
    public readonly String this[Int32 index] => _routes[index];

    /// <summary>
    /// Gets the route at the specified index using an index object.
    /// </summary>
    public readonly String this[Index index] => _routes[index];

    /// <summary>
    /// Gets a sub path using the specified range.
    /// </summary>
    public readonly EntryPath this[Range range] => new(_routes[range]);

    /// <summary>
    /// Get the number of routes in the EntryPath.
    /// </summary>
    public readonly Int32 Length => _routes.Length;

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    public readonly String StringPath => Path.Combine(_routes);

    /// <summary>
    /// Determines whether the current <see cref="EntryPath"/> is a sub path of the specified path.
    /// </summary>
    /// <param name="otherPath">The path to check if it contains the current path.</param>
    /// <returns><c>true</c> if the current path is a sub path of the specified path; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Determines whether the current <see cref="EntryPath"/> is a sub path of the specified path.
    /// </summary>
    /// <param name="otherPath">The path to check if it contains the current path.</param>
    /// <param name="routeEqualityComparer">The comparer to compare equality of string.</param>
    /// <returns><c>true</c> if the current path is a sub path of the specified path; otherwise, <c>false</c>.</returns>
    public Boolean IsSubPathOf(EntryPath otherPath, IEqualityComparer<String> routeEqualityComparer)
    {
        if (_routes.Length <= otherPath._routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < otherPath.Length; i++)
        {
            if (!routeEqualityComparer.Equals(_routes[i], otherPath[i]))
            {
                return false;
            }
        }

        return true;
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

    /// <summary>
    /// Creates an <see cref="EntryPath"/> instance from a string path.
    /// </summary>
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

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <summary>
/// Represents a path to a directory or file.
/// </summary>
public class EntryPath :
    IComparable<EntryPath>,
    IEquatable<EntryPath>
{
    /// <summary>
    /// Gets the route at the specified index.
    /// </summary>
    public String this[Int32 index] => _routes[index];

    /// <summary>
    /// Gets the route at the specified index using an index object.
    /// </summary>
    public String this[Index index] => _routes[index];

    /// <summary>
    /// Gets a subpath using the specified range.
    /// </summary>
    public EntryPath this[Range range] => new(_routes[range]);

    /// <summary>
    /// Get the number of routes in the EntryPath.
    /// </summary>
    public Int32 Length => _routes.Length;

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    public String StringPath => Path.Combine(_routes.ToArray());

    public override String ToString()
    {
        return String.Join("", _routes.Select(p => $"[{p}]"));
    }

    public override Boolean Equals([NotNullWhen(true)] Object? obj)
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

    public override Int32 GetHashCode()
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

    public Boolean Equals(EntryPath? other)
    {
        return this == other;
    }

    public Int32 CompareTo(EntryPath? other)
    {
        if (other is null)
        {
            return 1;
        }
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

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

    public static Boolean operator ==(EntryPath? left, EntryPath? right)
    {
        if (left is null && right is not null)
        {
            return false;
        }
        if (left is not null && right is null)
        {
            return false;
        }
        if (left is null && right is null)
        {
            return true;
        }
        if (ReferenceEquals(left, right))
        {
            return true;
        }

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

    public static Boolean operator !=(EntryPath? left, EntryPath? right)
    {
        return left == right;
    }

    /// <summary>
    /// Creates an EntryPath instance from a string path.
    /// </summary>
    public static EntryPath CreateFromPath(String path)
    {
        String normalizedPath = Path.TrimEndingDirectorySeparator(path);
        String[] routes = normalizedPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return new EntryPath(routes);
    }

    /// <summary>
    /// Initializes a new instance of the EntryPath struct with an array of routes.
    /// </summary>
    public EntryPath(params String[] routes)
    {
        _routes = routes.ToImmutableArray();
    }

    /// <summary>
    /// Initializes a new instance of the EntryPath struct with a collection of routes.
    /// </summary>
    public EntryPath(IEnumerable<String> routes)
    {
        _routes = routes.ToImmutableArray();
    }

    #region NonPublic
    private readonly ImmutableArray<String> _routes;
    #endregion
}
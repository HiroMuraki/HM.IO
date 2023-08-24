using HM.IO.RoutedItems;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <summary>
/// Represents a path to a directory or file.
/// </summary>
public readonly struct EntryPath :
    IEquatable<EntryPath>
{
    /// <summary>
    /// Gets the individual routes (components) that make up the path.
    /// </summary>
    public readonly ImmutableArray<String> Routes => _routes;

    /// <summary>
    /// Get the number of routes in the EntryPath.
    /// </summary>
    public readonly Int32 Length => _routes.Length;

    /// <summary>
    /// Gets the route at the specified index.
    /// </summary>
    public readonly String this[Int32 index] => _routes[index];

    /// <summary>
    /// Gets the route at the specified index using an index object.
    /// </summary>
    public readonly String this[Index index] => _routes[index];

    /// <summary>
    /// Gets a subpath using the specified range.
    /// </summary>
    public readonly EntryPath this[Range range] => new(RoutedItemHelper.Slice(in _routes, range));

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    public readonly String StringPath => Path.Combine(_routes.ToArray());

    /// <summary>
    /// Converts the EntryPath to a string representation.
    /// </summary>
    public override readonly String ToString()
    {
        return RoutedItemHelper.ToString(in _routes);
    }

    /// <summary>
    /// Determines whether this EntryPath is equal to another object.
    /// </summary>
    public override readonly Boolean Equals([NotNullWhen(true)] Object? obj)
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

    /// <summary>
    /// Computes the hash code for this EntryPath.
    /// </summary>
    public override readonly Int32 GetHashCode()
    {
        return RoutedItemHelper.GetHashCode(in _routes);
    }

    /// <summary>
    /// Determines whether this EntryPath is equal to another EntryPath.
    /// </summary>
    public readonly Boolean Equals(EntryPath other)
    {
        return this == other;
    }

    /// <summary>
    /// Checks if two EntryPath instances are equal.
    /// </summary>
    public static Boolean operator ==(EntryPath left, EntryPath right)
    {
        return RoutedItemHelper.Equals(in left._routes, in right._routes, StringComparer.Ordinal);
    }

    /// <summary>
    /// Checks if two EntryPath instances are not equal.
    /// </summary>
    public static Boolean operator !=(EntryPath left, EntryPath right)
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
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HM.IO;

/// <summary>
///     Represents a path to a directory or file.
/// </summary>
public readonly struct EntryPath :
    IEquatable<EntryPath>,
    IEqualityOperators<EntryPath, EntryPath, Boolean>,
    IComparable<EntryPath>,
    IComparable
{
    #region Properties
    /// <summary>
    ///     Gets an empty <see cref="EntryPath"/> instance.
    /// </summary>
    /// <value>
    ///     An empty <see cref="EntryPath"/> instance.
    /// </value>
    public static EntryPath Empty { get; } = new EntryPath(String.Empty);

    /// <summary>
    ///     Gets the route at the specified index.
    /// </summary>
    /// <param name="index">The index of the route to retrieve.</param>
    /// <value>
    ///     A <see cref="String" /> representing the route at the specified index.
    /// </value>
    public readonly String this[Int32 index]
    {
        get
        {
            return GetRoutes()[index];
        }
    }

    /// <summary>
    ///     Gets the route at the specified index using an index object.
    /// </summary>
    /// <param name="index">The index object indicating the route to retrieve.</param>
    /// <value>
    ///     A <see cref="String" /> representing the route at the specified index.
    /// </value>
    public readonly String this[Index index]
    {
        get
        {
            return GetRoutes()[index];
        }
    }

    /// <summary>
    ///     Gets a sub path using the specified range.
    /// </summary>
    /// <param name="range">The range specifying the sub path.</param>
    /// <value>
    ///     An <see cref="EntryPath" /> representing the sub path.
    /// </value>
    public readonly EntryPath this[Range range]
    {
        get
        {
            return new EntryPath(String.Join(SystemPathSeparatorChar, GetRoutes()[range]));
        }
    }

    public readonly String[] Routes => GetRoutes();

    /// <summary>
    ///     Gets the path as a string.
    /// </summary>
    /// <value>
    ///     A <see cref="String" /> representing its string-style path.
    /// </value>
    public readonly String StringPath
    {
        get
        {
            return _stringPath;
        }
    }

    /// <summary>
    ///     Gets the full path of current entry path.
    /// </summary>
    /// <value>
    ///     The full <see cref="EntryPath"/> of the entry.
    /// </value>
    public readonly EntryPath FullPath
    {
        get
        {
            return Create(Path.GetFullPath(StringPath));
        }
    }

    /// <summary>
    ///     Gets the root of current entry path.
    /// </summary>
    /// <value>
    ///     The root of the path, or <see langword="null"/> if the path is not rooted.
    /// </value>
    public readonly String? PathRoot
    {
        get
        {
            return Path.GetPathRoot(StringPath);
        }
    }

    /// <summary>
    ///     Gets the directory name part of the current entry path.
    /// </summary>
    /// <value>
    ///     An <see cref="EntryPath" /> representing the directory name part of the current entry path.
    /// </value>
    public readonly EntryPath DirectoryName
    {
        get
        {
            String[] routes = GetRoutes();
            return new EntryPath(String.Join(SystemPathSeparatorChar, routes[0..(routes.Length - 1)]));
        }
    }

    /// <summary>
    ///     Gets the entry name part of the current entry path.
    /// </summary>
    /// <value>
    ///     A string representing the entry name part of the current entry path.
    /// </value>
    public readonly String EntryName
    {
        get
        {
            return GetRoutes()[^1];
        }
    }

    /// <summary>
    ///     Gets the number of routes of current entry path.
    /// </summary>
    /// <value>
    ///     An <see cref="Int32" /> representing number of routes in the <see cref="EntryPath"/>.
    /// </value>
    public readonly Int32 LengthOfRoutes
    {
        get
        {
            return GetRoutes().Length;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    ///     Determines if the current entry path is a sub path of the specified other path.
    /// </summary>
    /// <param name="otherPath">The other entry path to compare against.</param>
    /// <returns>
    ///     <c>true</c> if the current entry path is a sub path of the other path; otherwise, <c>false</c>.
    /// </returns>
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

    /// <summary>
    ///     Determines if the current entry path is a parent path of the specified other path.
    /// </summary>
    /// <param name="otherPath">The other entry path to compare against.</param>
    /// <returns>
    ///     <c>true</c> if the current entry path is a parent path of the other path; otherwise, <c>false</c>.
    /// </returns>
    public readonly Boolean IsParentPathOf(EntryPath otherPath)
    {
        return otherPath.IsSubPathOf(this);
    }

    public readonly Boolean Equals(EntryPath other)
    {
        return this == other;
    }

    public readonly Int32 CompareTo(EntryPath other)
    {
        String[] routes = GetRoutes();
        String[] otherRoutes = other.GetRoutes();

        Int32 minLength = Int32.Min(routes.Length, otherRoutes.Length);

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = routes[i].CompareTo(otherRoutes[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (routes.Length < otherRoutes.Length)
        {
            return -1;
        }
        else if (routes.Length > otherRoutes.Length)
        {
            return 1;
        }

        return 0;
    }

    public readonly Int32 CompareTo(Object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        return CompareTo((EntryPath)obj);
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
                hashCode = HashCode.Combine(hashCode, routes[0].ToUpper()) * 31;
            }
            if (routes.Length > 1)
            {
                hashCode = HashCode.Combine(hashCode, routes[^1].ToUpper()) * 31;
            }

            return hashCode;
        }
    }

    /// <summary>
    ///     Creates an <see cref="EntryPath" /> instance from the specified path.
    /// </summary>
    /// <param name="path">The path <see cref="String" /> to create an <see cref="EntryPath" /> from.</param>
    /// <returns>
    ///     An <see cref="EntryPath" /> instance representing the specified path.
    /// </returns>
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

    /// <summary>
    ///     Combines two <see cref="EntryPath"/> instances.
    /// </summary>
    /// <param name="path1">The first <see cref="EntryPath"/>.</param>
    /// <param name="path2">The second <see cref="EntryPath"/>.</param>
    /// <returns>
    ///     A new <see cref="EntryPath"/> instance representing the combination of the specified paths.
    /// </returns>
    public static EntryPath Combine(EntryPath path1, EntryPath path2)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath));
    }

    /// <summary>
    ///     Combines three <see cref="EntryPath"/> instances.
    /// </summary>
    /// <param name="path1">The first <see cref="EntryPath"/>.</param>
    /// <param name="path2">The second <see cref="EntryPath"/>.</param>
    /// <param name="path3">The third <see cref="EntryPath"/>.</param>
    /// <returns>
    ///     A new <see cref="EntryPath"/> instance representing the combination of the specified paths.
    /// </returns>
    public static EntryPath Combine(EntryPath path1, EntryPath path2, EntryPath path3)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath, path3.StringPath));
    }

    /// <summary>
    ///     Combines four <see cref="EntryPath"/> instances.
    /// </summary>
    /// <param name="path1">The first <see cref="EntryPath"/>.</param>
    /// <param name="path2">The second <see cref="EntryPath"/>.</param>
    /// <param name="path3">The third <see cref="EntryPath"/>.</param>
    /// <param name="path4">The fourth <see cref="EntryPath"/>.</param>
    /// <returns>
    ///     A new <see cref="EntryPath"/> instance representing the combination of the specified paths.
    /// </returns>
    public static EntryPath Combine(EntryPath path1, EntryPath path2, EntryPath path3, EntryPath path4)
    {
        return Create(Path.Combine(path1.StringPath, path2.StringPath, path3.StringPath, path4.StringPath));
    }

    /// <summary>
    ///     Combines multiple <see cref="EntryPath"/> instances.
    /// </summary>
    /// <param name="paths">An <see cref="IEnumerable{T}"/> of <see cref="EntryPath"/> instances to combine.</param>
    /// <returns>
    ///     A new <see cref="EntryPath"/> instance representing the combination of the specified paths.
    /// </returns>
    public static EntryPath Combine(IEnumerable<EntryPath> paths)
    {
        return Create(Path.Combine(paths.Select(x => x.StringPath).ToArray()));
    }

    /// <summary>
    ///     Determines whether two <see cref="EntryPath"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="EntryPath"/> to compare.</param>
    /// <param name="right">The second <see cref="EntryPath"/> to compare.</param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="EntryPath"/> instances are equal; otherwise, <see langword="false"/>.
    /// </returns>
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

    /// <summary>
    ///     Determines whether two <see cref="EntryPath"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="EntryPath"/> to compare.</param>
    /// <param name="right">The second <see cref="EntryPath"/> to compare.</param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="EntryPath"/> instances are not equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator !=(EntryPath left, EntryPath right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Do not call this default constructor as it will throw an <see cref="InvalidOperationException"/> if called, use <see cref="Create(String)"/> Instead.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when an invalid operation is performed.</exception>
    public EntryPath()
    {
        throw new InvalidOperationException($"Unable to call default constructor of `{typeof(EntryPath)}`.");
    }
    #endregion

    #region NonPublic
    private static Char[] PathSeparatorChars { get; } = [SystemPathSeparatorChar, AltPathSeparatorChar];
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

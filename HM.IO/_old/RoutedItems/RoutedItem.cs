#if PREVIEW
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Previews.RoutedItems;

public class RoutedItem<TRoute> :
    IEquatable<RoutedItem<TRoute>>,
    IComparable<RoutedItem<TRoute>>,
    IComparable
    where TRoute :
    IEquatable<TRoute>,
    IComparable<TRoute>
{
    #region Properties
    /// <summary>
    /// Gets the individual routes (components) that make up the path.
    /// </summary>
    public ImmutableArray<TRoute> Routes { get; }

    /// <summary>
    /// Gets the route at the specified index.
    /// </summary>
    public TRoute this[Int32 index] => Routes[index];

    /// <summary>
    /// Gets the route at the specified index using an index object.
    /// </summary>
    public TRoute this[Index index] => Routes[index];

    /// <summary>
    /// Gets a subpath using the specified range.
    /// </summary>
    public RoutedItem<TRoute> this[Range range] => new(Routes[range]);

    /// <summary>
    /// Gets the path as a string.
    /// </summary>
    public virtual String StringPath => ToString();
    #endregion

    #region Methods
    public override String ToString()
    {
        return String.Join("", Routes.Select(p => $"[{p}]"));
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
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return Equals((EntryPath)obj);
    }

    public override Int32 GetHashCode()
    {
        unchecked
        {
            Int32 hashCode = Routes.Length ^ 17;
            if (Routes.Length > 0)
            {
                hashCode = HashCode.Combine(hashCode, Routes[0]) * 31;
            }
            if (Routes.Length > 1)
            {
                hashCode = HashCode.Combine(hashCode, Routes[^1]) * 31;
            }
            return hashCode;
        }
    }

    public virtual Boolean Equals(RoutedItem<TRoute>? other)
    {
        return this == other;
    }

    public virtual Int32 CompareTo(RoutedItem<TRoute>? other)
    {
        if (other is null)
        {
            return 1;
        }

        Int32 minLength = Routes.Length < other.Routes.Length ? Routes.Length : other.Routes.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = Routes[i].CompareTo(other.Routes[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (Routes.Length < other.Routes.Length)
        {
            return -1;
        }
        else if (Routes.Length > other.Routes.Length)
        {
            return 1;
        }

        return 0;
    }

    public virtual Int32 CompareTo(Object? obj)
    {
        return CompareTo(obj as RoutedItem<TRoute>);
    }

    public static Boolean operator ==(RoutedItem<TRoute>? left, RoutedItem<TRoute>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null && right is not null)
        {
            return false;
        }
        if (left is not null && right is null)
        {
            return false;
        }

        if (left!.Routes.Length != right!.Routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < right.Routes.Length; i++)
        {
            if (!left.Routes[i].Equals(right.Routes[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static Boolean operator !=(RoutedItem<TRoute>? left, RoutedItem<TRoute>? right)
    {
        return !(left == right);
    }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the EntryPath struct with an array of routes.
    /// </summary>
    public RoutedItem(TRoute[] routes)
    {
        Routes = routes.ToImmutableArray();
    }

    /// <summary>
    /// Initializes a new instance of the EntryPath struct with a collection of routes.
    /// </summary>
    public RoutedItem(IEnumerable<TRoute> routes)
    {
        Routes = routes.ToImmutableArray();
    }
    #endregion
}
#endif

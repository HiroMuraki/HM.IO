using System.Collections.Immutable;

namespace HM.IO.RoutedItems;

public static class RoutedItemHelper
{
    public static ImmutableArray<T> Slice<T>(in ImmutableArray<T> routes, in Range range)
    {
        return routes[range];
    }

    public static String ToString<T>(in ImmutableArray<T> routes)
    {
        return String.Join("", routes.Select(p => $"[{p}]"));
    }

    public static String ToString<T>(in T[] routes)
    {
        return String.Join("", routes.Select(p => $"[{p}]"));
    }

    public static Boolean Equals<T>(in ImmutableArray<T> x, in ImmutableArray<T> y, IEqualityComparer<T> equalityComparer)
    {
        if (x.Length != y.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < y.Length; i++)
        {
            if (!equalityComparer.Equals(x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static Boolean Equals<T>(in T[] x, in T[] y, IEqualityComparer<T> equalityComparer)
    {
        if (x.Length != y.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < y.Length; i++)
        {
            if (!equalityComparer.Equals(x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static Int32 GetHashCode<T>(in ImmutableArray<T> routes)
    {
        unchecked
        {
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

    public static Int32 GetHashCode<T>(in T[] routes)
    {
        unchecked
        {
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

    public static Int32 Compare<T>(in ImmutableArray<T> x, in ImmutableArray<T> y, IComparer<T> comparer)
    {
        Int32 minLength = x.Length < y.Length ? x.Length : y.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = comparer.Compare(x[i], y[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (x.Length < y.Length)
        {
            return -1;
        }
        else if (x.Length > y.Length)
        {
            return 1;
        }

        return 0;
    }

    public static Int32 Compare<T>(in T[] x, in T[] y, IComparer<T> comparer)
    {
        Int32 minLength = x.Length < y.Length ? x.Length : y.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = comparer.Compare(x[i], y[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (x.Length < y.Length)
        {
            return -1;
        }
        else if (x.Length > y.Length)
        {
            return 1;
        }

        return 0;
    }
}

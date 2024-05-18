namespace HM.Common;

public static class ComparisonHelper
{
    public static Boolean StructEquals<T>(T x, Object? y)
        where T : struct, IEquatable<T>
    {
        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Equals((T)y);
    }

    public static Int32 StructCompareTo<T>(T x, Object? y)
        where T : struct, IComparable<T>
    {
        if (y is null)
        {
            return 1;
        }

        return x.CompareTo((T)y);
    }

    public static Boolean ClassEquals<T>(T x, Object? y)
        where T : class, IEquatable<T>
    {
        if (y is null)
        {
            return x is null;
        }

        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Equals((T)y);
    }

    public static Boolean ClassIEquatableEquals<T>(T x, T? y, IEqualityComparer<T> equalityComparer)
        where T : class
    {
        if (y is null)
        {
            return x is null;
        }

        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return equalityComparer.Equals(x, y);
    }
}

//using System.Diagnostics.CodeAnalysis;

//namespace HM.IO.RoutedItems;

//public interface IRouteComparer<TRoute> :
//    IComparer<RoutedItem<TRoute>>,
//    IEqualityComparer<RoutedItem<TRoute>>
//    where TRoute :
//    IEquatable<TRoute>,
//    IComparable<TRoute>
//{

//}

//public class RouteComparer<TRoute> :
//    IRouteComparer<TRoute>
//    where TRoute :
//    IEquatable<TRoute>,
//    IComparable<TRoute>
//{
//    public virtual Int32 Compare(RoutedItem<TRoute>? x, RoutedItem<TRoute>? y)
//    {

//    }
//    public virtual Boolean Equals(RoutedItem<TRoute>? x, RoutedItem<TRoute>? y)
//    {

//    }
//    public virtual Int32 GetHashCode([DisallowNull] RoutedItem<TRoute> obj)
//    {
//    }
//}

//public interface IRoutedItemComparer<T>
//    : IComparer<RoutedItem<T>>, IEqualityComparer<RoutedItem<T>>
//    where T : IEquatable<T>, IComparable<T>
//{

//}

///// <summary>
///// Provides an equality comparer for <see cref="EntryPath"/> instances.
///// </summary>
//public sealed class RoutedItemComparer<T>
//    : IRoutedItemComparer<T>
//    where T : IEquatable<T>, IComparable<T>
//{
//    /// <summary>
//    /// Gets the default instance of the <see cref="EntryPathComparer"/>.
//    /// </summary>
//    public static EntryPathComparer Default { get; } = new();

//    public IRouteComparer<T> RouteComparer { get; set; }

//    public Boolean Equals(RoutedItem<T>? x, RoutedItem<T>? y)
//    {
//        if (x.Routes.Length != y.Routes.Length)
//        {
//            return false;
//        }

//        for (Int32 i = 0; i < x.Routes.Length; i++)
//        {
//            if (!RouteComparer.Equals(x.Routes[i], y.Routes[i]))
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    public Int32 GetHashCode([DisallowNull] RoutedItem<T> obj)
//    {
//        unchecked
//        {
//            Int32 hashCode = obj.Routes.Length ^ 17;
//            if (obj.Routes.Length > 0)
//            {
//                hashCode = HashCode.Combine(hashCode, obj.Routes[0]) * 31;
//            }
//            if (obj.Routes.Length > 1)
//            {
//                hashCode = HashCode.Combine(hashCode, obj.Routes[^1]) * 31;
//            }
//            return hashCode;
//        }
//    }

//    public Int32 Compare(RoutedItem<T>? x, RoutedItem<T>? y)
//    {
//        Int32 minLength = x.Routes.Length < y.Routes.Length ? x.Routes.Length : y.Routes.Length;

//        for (Int32 i = 0; i < minLength; i++)
//        {
//            Int32 compareResult = RouteComparer.Compare(x.Routes[i], y.Routes[i]);
//            if (compareResult < 0)
//            {
//                return -1;
//            }
//            else if (compareResult > 0)
//            {
//                return 1;
//            }
//        }

//        if (x.Routes.Length < y.Routes.Length)
//        {
//            return -1;
//        }
//        else if (x.Routes.Length > y.Routes.Length)
//        {
//            return 1;
//        }

//        return 0;
//    }
//}

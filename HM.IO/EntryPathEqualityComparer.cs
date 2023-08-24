using HM.IO.RoutedItems;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <summary>
/// Provides an equality comparer for <see cref="EntryPath"/> instances.
/// </summary>
public sealed class EntryPathEqualityComparer
    : IEntryPathEqualityComparer
{
    /// <summary>
    /// Gets the default instance of the <see cref="EntryPathEqualityComparer"/>.
    /// </summary>
    public static EntryPathEqualityComparer Default { get; } = new();

    public IEqualityComparer<String> RouteEqualityComparer { get; set; } = new RouteEqualityComparer();

    public Boolean Equals(EntryPath x, EntryPath y)
    {
        return RoutedItemHelper.Equals(x.Routes, y.Routes, RouteEqualityComparer);
    }

    public Int32 GetHashCode([DisallowNull] EntryPath obj)
    {
        return RoutedItemHelper.GetHashCode(obj.Routes);
    }
}

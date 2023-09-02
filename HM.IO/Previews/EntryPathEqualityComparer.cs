#if PREVIEW
using HM.IO.RoutedItems;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Previews;

/// <summary>
/// Provides an equality comparer for <see cref="EntryPath"/> instances.
/// </summary>
public sealed class EntryPathEqualityComparer
    : IEntryPathEqualityComparer
{
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
#endif
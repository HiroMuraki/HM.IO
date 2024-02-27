#if PREVIEW
using HM.IO.RoutedItems;

namespace HM.IO.Previews;

/// <summary>
/// Provides a comparer for <see cref="EntryPath"/> instances.
/// </summary>
public sealed class EntryPathComparer
    : IEntryPathComparer
{
    public IComparer<String> RouteComparer { get; set; } = new RouteComparer();

    public Int32 Compare(EntryPath x, EntryPath y)
    {
        return RoutedItemHelper.Compare(x.Routes, y.Routes, RouteComparer);
    }
}
#endif
#if PREVIEW
namespace HM.IO.Previews.RoutedItems;

public sealed class RoutedString : RoutedItem<String>
{
    public RoutedString(String[] routes) : base(routes)
    {
    }
}
#endif
#if PREVIEW
namespace HM.IO.Previews;

public interface IEntryPathComparer :
    IComparer<EntryPath>
{
    public IComparer<String> RouteComparer { get; }
}
#endif
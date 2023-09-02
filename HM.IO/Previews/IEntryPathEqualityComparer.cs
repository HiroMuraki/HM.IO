#if PREVIEW
namespace HM.IO.Previews;

public interface IEntryPathEqualityComparer :
    IEqualityComparer<EntryPath>
{
    public IEqualityComparer<String> RouteEqualityComparer { get; }
}
#endif
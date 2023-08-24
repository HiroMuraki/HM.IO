namespace HM.IO;

public interface IEntryPathEqualityComparer :
    IEqualityComparer<EntryPath>
{
    public IEqualityComparer<String> RouteEqualityComparer { get; }
}

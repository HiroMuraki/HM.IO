namespace HM.IO;

public interface IEntryPathComparer
    : IComparer<EntryPath>, IEqualityComparer<EntryPath>
{
    IRouteComparer RouteComparer { get; set; }
}

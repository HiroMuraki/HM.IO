using System.Numerics;

namespace HM.IO.Previews;

public interface IEntryPath<TEntryName> :
    IEquatable<TEntryName>,
    IEqualityOperators<TEntryName, TEntryName, Boolean>,
    IComparable<TEntryName>,
    IComparable
    where TEntryName : struct, IEntryPath<TEntryName>
{
    abstract static TEntryName Empty { get; }

    String StringPath { get; }
}

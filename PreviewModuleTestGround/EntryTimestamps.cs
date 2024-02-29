using HM.Common;
using System.Numerics;

namespace HM.IO.Previews;

public readonly struct EntryTimestamps :
    IEquatable<EntryTimestamps>,
    IEqualityOperators<EntryTimestamps, EntryTimestamps, Boolean>
{
    public readonly DateTime CreationTime { get; init; }

    public readonly DateTime LastWriteTime { get; init; }

    public readonly DateTime LastAccessTime { get; init; }

    public readonly Boolean Equals(EntryTimestamps other)
    {
        return CreationTime == other.CreationTime
            && LastWriteTime == other.LastWriteTime
            && LastAccessTime == other.LastAccessTime;
    }

    public readonly override Boolean Equals(Object? obj)
       => ComparisonHelper.StructEquals(this, obj);

    public readonly override Int32 GetHashCode()
    {
        return HashCode.Combine(CreationTime, LastWriteTime, LastAccessTime);
    }

    public static Boolean operator ==(EntryTimestamps left, EntryTimestamps right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(EntryTimestamps left, EntryTimestamps right)
    {
        return !(left == right);
    }

    public override String ToString()
    {
        throw new NotSupportedException();
    }
}
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public readonly struct EntryPath
    : IEquatable<EntryPath>, IComparable<EntryPath>
{
    public readonly ImmutableArray<String> Routes { get; }
    public readonly String this[Int32 index] => Routes[index];
    public readonly String this[Index index] => Routes[index];
    public readonly EntryPath this[Range range] => new(Routes[range]);
    public readonly String StringPath => Path.Combine(Routes.ToArray());

    public override readonly String ToString()
    {
        return String.Join("", Routes.Select(p => $"[{p}]"));
    }

    public override readonly Boolean Equals([NotNullWhen(true)] Object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (GetType() != obj.GetType())
        {
            return false;
        }

        return Equals((EntryPath)obj);
    }

    public override readonly Int32 GetHashCode()
    {
        Int32 hashCode = Routes.Length ^ 17;
        if (Routes.Length > 0)
        {
            hashCode = HashCode.Combine(hashCode, Routes[0]) * 31;
        }
        if (Routes.Length > 1)
        {
            hashCode = HashCode.Combine(hashCode, Routes[^1]) * 31;
        }
        return hashCode;
    }

    public readonly Boolean Equals(EntryPath other)
    {
        return this == other;
    }

    public readonly Int32 CompareTo(EntryPath other)
    {
        Int32 minLength = Routes.Length < other.Routes.Length ? Routes.Length : other.Routes.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = String.Compare(Routes[i], other.Routes[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (Routes.Length < other.Routes.Length)
        {
            return -1;
        }
        else if (Routes.Length > other.Routes.Length)
        {
            return 1;
        }

        return 0;
    }

    public static Boolean operator ==(EntryPath left, EntryPath right)
    {
        if (left.Routes.Length != right.Routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < right.Routes.Length; i++)
        {
            if (left.Routes[i] == right.Routes[i])
            {
                return false;
            }
        }

        return true;
    }

    public static Boolean operator !=(EntryPath left, EntryPath right)
    {
        return left == right;
    }

    public static EntryPath CreateFromPath(String path)
    {
        String normalizedPath = Path.TrimEndingDirectorySeparator(path);
        String[] routes = normalizedPath.Split('\\', '/');
        return new EntryPath(routes);
    }

    public EntryPath(String[] routes)
    {
        Routes = routes.ToImmutableArray();
    }
              
    public EntryPath(IEnumerable<String> routes)
    {
        Routes = routes.ToImmutableArray();
    }
}

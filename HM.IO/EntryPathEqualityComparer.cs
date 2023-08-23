using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public sealed class EntryPathEqualityComparer : IEqualityComparer<EntryPath>
{
    public static EntryPathEqualityComparer Default { get; } = new();

    public Boolean IgnoreCase
    {
        get => _ignoreCase;
        set
        {
            _ignoreCase = value;
            if (_ignoreCase)
            {
                _comparer = StringComparer.OrdinalIgnoreCase;
            }
            else
            {
                _comparer = StringComparer.Ordinal;
            }
        }
    }

    public Boolean Equals(EntryPath x, EntryPath y)
    {
        if (x.Routes.Length != y.Routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < x.Routes.Length; i++)
        {
            if (!_comparer.Equals(x.Routes[i], y.Routes[i]))
            {
                return false;
            }
        }

        return true;
    }

    public Int32 GetHashCode([DisallowNull] EntryPath obj)
    {
        Int32 hashCode = obj.Routes.Length ^ 17;
        if (obj.Routes.Length > 0)
        {
            hashCode = HashCode.Combine(hashCode, obj.Routes[0]) * 31;
        }
        if (obj.Routes.Length > 1)
        {
            hashCode = HashCode.Combine(hashCode, obj.Routes[^1]) * 31;
        }
        return hashCode;
    }

    public EntryPathEqualityComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IEqualityComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}

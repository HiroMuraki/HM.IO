using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public class EntryPathEqualityComparer :
    EqualityComparer<EntryPath>
{
    public override Boolean Equals(EntryPath x, EntryPath y)
       => Equals(x, y, GetPlatformRelativeStringComparison());

    public Boolean Equals(EntryPath x, EntryPath y, StringComparison routeStringComparison)
        => Equals(x, y, GetStringComparer(routeStringComparison));

    public Boolean Equals(EntryPath x, EntryPath y, StringComparer routeStringComparer)
    {
        String[] routesOfLeft = x.Routes;
        String[] routesOfRight = y.Routes;

        if (routesOfLeft.Length != routesOfRight.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < routesOfLeft.Length; i++)
        {
            if (routeStringComparer.Compare(routesOfLeft[i], routesOfRight[i]) != 0)
            {
                return false;
            }
        }

        return true;
    }

    public override Int32 GetHashCode([DisallowNull] EntryPath obj)
    {
        return obj.GetHashCode();
    }

    #region NonPublic
    private static StringComparison GetPlatformRelativeStringComparison()
    {
        StringComparison stringComparison = StringComparison.Ordinal;

        if (OperatingSystem.IsWindows())
        {
            stringComparison = StringComparison.OrdinalIgnoreCase;
        }

        return stringComparison;
    }
    private static StringComparer GetStringComparer(StringComparison stringComparison)
    {
        switch (stringComparison)
        {
            case StringComparison.CurrentCulture:
                return StringComparer.CurrentCulture;
            case StringComparison.CurrentCultureIgnoreCase:
                return StringComparer.CurrentCultureIgnoreCase;
            case StringComparison.InvariantCulture:
                return StringComparer.InvariantCulture;
            case StringComparison.InvariantCultureIgnoreCase:
                return StringComparer.InvariantCultureIgnoreCase;
            case StringComparison.Ordinal:
                return StringComparer.Ordinal;
            case StringComparison.OrdinalIgnoreCase:
                return StringComparer.OrdinalIgnoreCase;
            default:
                throw new ArgumentOutOfRangeException($"{stringComparison} is not a valid value of {nameof(StringComparison)}.");
        }
    }
    #endregion
}

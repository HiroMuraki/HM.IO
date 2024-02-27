namespace HM.IO;

public class EntryPathComparer :
    Comparer<EntryPath>
{
    public override Int32 Compare(EntryPath x, EntryPath y)
        => Compare(x, y, GetPlatformRelativeStringComparison());

    public Int32 Compare(EntryPath x, EntryPath y, StringComparison routeStringComparison)
        => Compare(x, y, GetStringComparer(routeStringComparison));

    public Int32 Compare(EntryPath x, EntryPath y, StringComparer routeStringComparer)
    {
        String[] xRoutes = x.Routes;
        String[] yRoutes = y.Routes;

        Int32 minLength = xRoutes.Length < yRoutes.Length ? xRoutes.Length : yRoutes.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = routeStringComparer.Compare(xRoutes[i], yRoutes[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (xRoutes.Length < yRoutes.Length)
        {
            return -1;
        }
        else if (xRoutes.Length > yRoutes.Length)
        {
            return 1;
        }

        return 0;
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

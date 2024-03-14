namespace HM.IO;

public class EntryPathComparer :
    Comparer<EntryPath>
{
    public override Int32 Compare(EntryPath x, EntryPath y)
        => Compare(x, y, GetPlatformRelativeStringComparison());

    public Int32 Compare(EntryPath x, EntryPath y, StringComparison stringPathComparison)
        => Compare(x, y, GetStringComparer(stringPathComparison));

    public Int32 Compare(EntryPath x, EntryPath y, StringComparer stringPathComparer)
    {
        return stringPathComparer.Compare(x.StringPath, y.StringPath);
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

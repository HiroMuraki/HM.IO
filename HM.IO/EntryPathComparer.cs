namespace HM.IO;

public sealed class EntryPathComparer : IComparer<EntryPath>
{
    public static EntryPathComparer Default { get; } = new();

    public Int32 Compare(EntryPath x, EntryPath y)
    {
        Int32 minLength = x.Routes.Length < y.Routes.Length ? x.Routes.Length : y.Routes.Length;

        for (Int32 i = 0; i < minLength; i++)
        {
            Int32 compareResult = String.Compare(x.Routes[i], y.Routes[i]);
            if (compareResult < 0)
            {
                return -1;
            }
            else if (compareResult > 0)
            {
                return 1;
            }
        }

        if (x.Routes.Length < y.Routes.Length)
        {
            return -1;
        }
        else if (x.Routes.Length > y.Routes.Length)
        {
            return 1;
        }

        return 0;
    }
}
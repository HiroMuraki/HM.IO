namespace HM.IO;

/// <summary>
/// Provides a comparer for sorting <see cref="EntryPath"/> instances.
/// </summary>
public sealed class EntryPathComparer : IComparer<EntryPath>
{
    /// <summary>
    /// Gets the default instance of the <see cref="EntryPathComparer"/>.
    /// </summary>
    public static EntryPathComparer Default { get; } = new();

    /// <summary>
    /// Compares two <see cref="EntryPath"/> instances.
    /// </summary>
    /// <param name="x">The first <see cref="EntryPath"/> to compare.</param>
    /// <param name="y">The second <see cref="EntryPath"/> to compare.</param>
    /// <returns>
    /// A value indicating the relative order of the <paramref name="x"/> and <paramref name="y"/> parameters.
    /// </returns>
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
namespace HM.IO;

/// <summary>
/// Provides extension methods for <see cref="EntryPath"/> instances.
/// </summary>
public static class EntryPathExtensions
{
    /// <summary>
    /// Determines whether the current <see cref="EntryPath"/> is a subpath of the specified path.
    /// </summary>
    /// <param name="self">The current <see cref="EntryPath"/> instance.</param>
    /// <param name="path">The path to check if it contains the current path.</param>
    /// <returns><c>true</c> if the current path is a subpath of the specified path; otherwise, <c>false</c>.</returns>
    public static Boolean IsSubPathOf(this EntryPath self, EntryPath path)
    {
        if (self.Length <= path.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < path.Length; i++)
        {
            if (self[i] != path[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the current <see cref="EntryPath"/> is a sub path of the specified path.
    /// </summary>
    /// <param name="self">The current <see cref="EntryPath"/> instance.</param>
    /// <param name="path">The path to check if it contains the current path.</param>
    /// <param name="routeEqualityComparer">The comparer to compare equality of string.</param>
    /// <returns><c>true</c> if the current path is a sub path of the specified path; otherwise, <c>false</c>.</returns>
    public static Boolean IsSubPathOf(this EntryPath self, EntryPath path, IEqualityComparer<String> routeEqualityComparer)
    {
        if (self.Length <= path.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < path.Length; i++)
        {
            if (!routeEqualityComparer.Equals(self[i], path[i]))
            {
                return false;
            }
        }

        return true;
    }
}

using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

/// <summary>
/// Provides an equality comparer for <see cref="EntryPath"/> instances.
/// </summary>
public sealed class EntryPathEqualityComparer : IEqualityComparer<EntryPath>
{
    /// <summary>
    /// Gets the default instance of the <see cref="EntryPathEqualityComparer"/>.
    /// </summary>
    public static EntryPathEqualityComparer Default { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the comparison should be case-insensitive.
    /// </summary>
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

    /// <summary>
    /// Determines whether two <see cref="EntryPath"/> instances are equal.
    /// </summary>
    /// <param name="x">The first <see cref="EntryPath"/> to compare.</param>
    /// <param name="y">The second <see cref="EntryPath"/> to compare.</param>
    /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Computes the hash code for a given <see cref="EntryPath"/> instance.
    /// </summary>
    /// <param name="obj">The <see cref="EntryPath"/> instance for which to compute the hash code.</param>
    /// <returns>The hash code for the specified object.</returns>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryPathEqualityComparer"/> class.
    /// </summary>
    public EntryPathEqualityComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IEqualityComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}

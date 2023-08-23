using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public class RouteComparer : IRouteComparer
{
    /// <summary>
    /// Gets the default instance of the <see cref="EntryPathComparer"/>.
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

    public Int32 Compare(String? x, String? y)
    {
        return _comparer.Compare(x, y);
    }

    public Boolean Equals(String? x, String? y)
    {
        return _comparer.Compare(x, y) == 0;
    }

    /// <summary>
    /// Computes the hash code for a given <see cref="String"/> instance.
    /// </summary>
    /// <param name="obj">The <see cref="String"/> instance for which to compute the hash code.</param>
    /// <returns>The hash code for the specified object.</returns>
    public Int32 GetHashCode([DisallowNull] String obj)
    {
        return _comparer.GetHashCode();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteComparer"/> class.
    /// </summary>
    public RouteComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}

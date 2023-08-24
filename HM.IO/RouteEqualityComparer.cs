using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public class RouteEqualityComparer :
    IEqualityComparer<String>
{
    /// <summary>
    /// Gets the default instance of the <see cref="EntryPathEqualityComparer"/>.
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

    public Boolean Equals(String? x, String? y)
    {
        return _comparer.Equals(x, y);
    }

    public Int32 GetHashCode([DisallowNull] String obj)
    {
        return _comparer.GetHashCode();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntryPathRouteComparer"/> class.
    /// </summary>
    public RouteEqualityComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IEqualityComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}

#if PREVIEW
using System.Diagnostics.CodeAnalysis;

namespace HM.IO.Previews;

public class RouteEqualityComparer :
    IEqualityComparer<String>
{
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

    public RouteEqualityComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IEqualityComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}

public class CompressedEntryPathsInfo
{
    public List<String> CompressedEntryPaths { get; init; } = new();

    public Dictionary<Int32, String> IdPathMap { get; init; } = new();
}
#endif
#if PREVIEW
namespace HM.IO.Previews;

public class RouteComparer : IComparer<String>
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

    public Int32 Compare(String? x, String? y)
    {
        return _comparer.Compare(x, y);
    }

    public RouteComparer()
    {
        IgnoreCase = OperatingSystem.IsWindows();
    }

    #region NonPublic
    private Boolean _ignoreCase;
    private IComparer<String> _comparer = StringComparer.Ordinal;
    #endregion
}
#endif
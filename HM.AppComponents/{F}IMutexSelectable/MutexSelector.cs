using HM.Common;

namespace HM.AppComponents;

public sealed class MutexSelector
{
    public Boolean AllowUnselectAll { get; set; } = false;

    internal void SetSelected(IMutexSelectable item)
    {
        Option<IMutexSelectable> previousSelected = _currentSelected;
        _currentSelected = new Option<IMutexSelectable>(item);
        previousSelected.GetThen(ps =>
        {
            if (ReferenceEquals(ps, item))
            {
                return CallChainState.Stop;
            }
            else
            {
                ps.IsMutexSelected = false;
                return CallChainState.Continue;
            }
        }).Continue(() =>
        {
            item.IsMutexSelected = true;
        });
    }

    internal void SetUnselected(IMutexSelectable item)
    {
        _currentSelected.GetThen(currentSelected =>
        {
            if (ReferenceEquals(currentSelected, item))
            {
                if (!AllowUnselectAll)
                {
                    return CallChainState.Stop;
                }

                currentSelected.IsMutexSelected = false;
                _currentSelected = null;
            }

            return CallChainState.Continue;
        }).Continue(() =>
        {
            item.IsMutexSelected = false;
        });
    }

    #region NonPublic
    private Option<IMutexSelectable> _currentSelected;
    #endregion
}

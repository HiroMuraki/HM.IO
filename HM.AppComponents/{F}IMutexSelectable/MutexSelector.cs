namespace HM.AppComponents;

public sealed class MutexSelector
{
    public Boolean AllowUnselectAll { get; set; } = false;

    internal void SetSelected(IMutexSelectable item)
    {
        IMutexSelectable? previousSelected = _currentSelected;
        _currentSelected = item;
        if (previousSelected is not null)
        {
            if (ReferenceEquals(previousSelected, item))
            {
                return;
            }
            else
            {
                previousSelected.IsMutexSelected = false;
            }
        }

        item.IsMutexSelected = true;
    }

    internal void SetUnselected(IMutexSelectable item)
    {
        if (_currentSelected is not null)
        {
            if (ReferenceEquals(_currentSelected, item))
            {
                if (!AllowUnselectAll)
                {
                    return;
                }

                _currentSelected.IsMutexSelected = false;
                _currentSelected = null;
            }
        }

        item.IsMutexSelected = false;
    }

    #region NonPublic
    private IMutexSelectable? _currentSelected;
    #endregion
}

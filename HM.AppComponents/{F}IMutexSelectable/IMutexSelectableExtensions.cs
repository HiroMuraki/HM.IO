namespace HM.AppComponents;

public static class IMutexSelectableExtensions
{
    public static void MutexSelect(this IMutexSelectable self, MutexSelector mutexSelector)
    {
        mutexSelector.SetSelected(self);
    }

    public static void MutexUnselect(this IMutexSelectable self, MutexSelector mutexSelector)
    {
        mutexSelector.SetUnselected(self);
    }
}

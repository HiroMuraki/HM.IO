namespace HM.AppService.Services;

public sealed class ProcessNotifier :
    IProcessNotifier
{
    public static ProcessNotifier Create(Action<Int32, Int32, String> action)
    {
        return new ProcessNotifier(action);
    }

    public void NotifyProcess(Int32 current, Int32 totalCount, String processDescription)
    {
        _action(current, totalCount, processDescription);
    }

    #region NonPublic
    private readonly Action<Int32, Int32, String> _action;
    public ProcessNotifier(Action<Int32, Int32, String> action)
    {
        _action = action;
    }
    #endregion
}
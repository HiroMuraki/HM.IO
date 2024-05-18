namespace HM.AppComponents.AppService.Services.Generic;

public sealed class ProcessNotifier<TArg> :
     IProcessNotifier<TArg>
{
    public static ProcessNotifier<TArg> Create(Action<TArg> action)
    {
        return new ProcessNotifier<TArg>(action);
    }

    public void NotifyProcess(TArg arg)
    {
        _action(arg);
    }

    #region NonPublic
    private readonly Action<TArg> _action;
    public ProcessNotifier(Action<TArg> action)
    {
        _action = action;
    }
    #endregion
}
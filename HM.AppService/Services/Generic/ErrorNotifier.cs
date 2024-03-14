namespace HM.AppService.Services.Generic;

public sealed class ErrorNotifier<TArg> :
    IErrorNotifier<TArg>
{
    public static ErrorNotifier<TArg> Create(Action<TArg> action)
    {
        return new ErrorNotifier<TArg>(action);
    }

    public void NotifyError(TArg arg)
    {
        _action(arg);
    }

    #region NonPublic
    private readonly Action<TArg> _action;
    public ErrorNotifier(Action<TArg> action)
    {
        _action = action;
    }
    #endregion
}
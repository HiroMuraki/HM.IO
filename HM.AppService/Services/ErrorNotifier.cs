namespace HM.AppService.Services;

public sealed class ErrorNotifier :
    IErrorNotifier
{
    public static ErrorNotifier Create(Action<String> action)
    {
        return new ErrorNotifier(action);
    }

    public void NotifyError(String message)
    {
        _action(message);
    }

    #region NonPublic
    private readonly Action<String> _action;
    private ErrorNotifier(Action<String> action)
    {
        _action = action;
    }
    #endregion
}
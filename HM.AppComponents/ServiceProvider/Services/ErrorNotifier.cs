namespace HM.AppComponents.AppService.Services;

public sealed class ErrorNotifier :
    IErrorNotifier
{
    public static ErrorNotifier Create(Action<Exception> action)
    {
        return new ErrorNotifier(action);
    }

    public void NotifyError(Exception exception)
    {
        _action(exception);
    }

    #region NonPublic
    private readonly Action<Exception> _action;
    private ErrorNotifier(Action<Exception> action)
    {
        _action = action;
    }
    #endregion
}
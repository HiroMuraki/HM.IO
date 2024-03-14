namespace HM.AppService.Services;

public sealed class Logger :
    ILogger
{
    public static Logger Create(Action<String> action)
    {
        return new Logger(action);
    }

    public void WriteLine(String message)
    {
        _action(message);
    }

    #region NonPublic
    private readonly Action<String> _action;
    public Logger(Action<String> action)
    {
        _action = action;
    }
    #endregion
}
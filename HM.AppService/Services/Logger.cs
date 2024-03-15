namespace HM.AppService.Services;

public sealed class Logger :
    ILogger
{
    public static Logger Create(Action<String, LogSeverity> action)
    {
        return new Logger(action);
    }

    public void WriteLine(String message, LogSeverity severity)
    {
        _action(message, severity);
    }

    #region NonPublic
    private readonly Action<String, LogSeverity> _action;
    public Logger(Action<String, LogSeverity> action)
    {
        _action = action;
    }
    #endregion
}
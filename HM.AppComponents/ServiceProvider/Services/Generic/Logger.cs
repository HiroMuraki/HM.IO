namespace HM.AppComponents.AppService.Services.Generic;

public sealed class Logger<TArg> :
    ILogger<TArg>
{
    public static Logger<TArg> Create(Action<TArg?> action)
    {
        return new Logger<TArg>(action);
    }

    public void WriteLine(TArg? arg)
    {
        _action(arg);
    }

    #region NonPublic
    private readonly Action<TArg?> _action;
    public Logger(Action<TArg?> action)
    {
        _action = action;
    }
    #endregion
}
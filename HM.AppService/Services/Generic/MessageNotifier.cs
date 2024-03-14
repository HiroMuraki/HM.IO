namespace HM.AppService.Services.Generic;

public sealed class MessageNotifier<TArg> :
     IMessageNotifier<TArg>
{
    public static MessageNotifier<TArg> Create(Action<TArg> action)
    {
        return new MessageNotifier<TArg>(action);
    }

    public void NotifyMessage(TArg arg)
    {
        _action(arg);
    }

    #region NonPublic
    private readonly Action<TArg> _action;
    public MessageNotifier(Action<TArg> action)
    {
        _action = action;
    }
    #endregion
}
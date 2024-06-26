﻿namespace HM.AppComponents.AppService.Services;

public sealed class MessageNotifier :
    IMessageNotifier
{
    public static MessageNotifier Create(Action<String> action)
    {
        return new MessageNotifier(action);
    }

    public void NotifyMessage(String message)
    {
        _action(message);
    }

    #region NonPublic
    private readonly Action<String> _action;
    private MessageNotifier(Action<String> action)
    {
        _action = action;
    }
    #endregion
}
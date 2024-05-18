namespace HM.AppComponents.AppService.Services.Generic;

public interface IMessageNotifier<TArg>
{
    void NotifyMessage(TArg arg);
}

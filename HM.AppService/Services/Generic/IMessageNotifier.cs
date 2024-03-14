namespace HM.AppService.Services.Generic;
public interface IMessageNotifier<TArgs>
{
    void Notify(TArgs args);
}
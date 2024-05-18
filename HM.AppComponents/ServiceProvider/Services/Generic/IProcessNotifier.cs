namespace HM.AppComponents.AppService.Services.Generic;

public interface IProcessNotifier<TArg>
{
    void NotifyProcess(TArg arg);
}

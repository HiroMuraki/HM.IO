namespace HM.AppComponents.AppService.Services.Generic;

public interface IErrorNotifier<TArg>
{
    void NotifyError(TArg arg);
}

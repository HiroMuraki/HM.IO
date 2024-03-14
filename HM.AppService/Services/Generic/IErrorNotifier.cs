namespace HM.AppService.Services.Generic;

public interface IErrorNotifier<TArg>
{
    void NotifyError(TArg arg);
}

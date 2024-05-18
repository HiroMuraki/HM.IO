namespace HM.AppComponents.AppService.Services.Generic;

public interface ILogger<TArg>
{
    void WriteLine(TArg arg);
}

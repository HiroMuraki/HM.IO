namespace HM.AppService.Services.Generic;

public interface ILogger<TArg>
{
    void WriteLine(TArg arg);
}

namespace HM.AppService.Services.Generic;

public interface ILogger<TArgs>
{
    void Log(TArgs args);
}

namespace HM.AppComponents.AppService.Services.Generic;

public interface IAppConfigProvider<TConfig>
    where TConfig : class
{
    TConfig? GetConfig();
}

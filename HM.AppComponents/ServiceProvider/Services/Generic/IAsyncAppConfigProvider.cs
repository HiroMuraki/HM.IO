namespace HM.AppComponents.AppService.Services.Generic;

public interface IAsyncAppConfigProvider<TConfig>
    where TConfig : class
{
    Task<TConfig?> GetConfigAsync();
}

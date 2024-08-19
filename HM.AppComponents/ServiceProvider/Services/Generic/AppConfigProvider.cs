namespace HM.AppComponents.AppService.Services.Generic;

public class AppConfigProvider<TConfig> :
    IAppConfigProvider<TConfig>
    where TConfig : class
{
    public static IAppConfigProvider<TConfig> Create(Func<TConfig?> func)
    {
        return new AppConfigProvider<TConfig>(func);
    }

    public TConfig? GetConfig()
    {
        return _delegate();
    }

    #region NonPublic
    private AppConfigProvider(Func<TConfig?> @delegate)
    {
        _delegate = @delegate;
    }
    private readonly Func<TConfig?> _delegate;
    #endregion
}

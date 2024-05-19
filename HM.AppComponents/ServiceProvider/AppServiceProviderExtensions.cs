using HM.Common;

namespace HM.AppComponents.AppService;

public static class AppServiceProviderExtensions
{
    public static CallChain<T?> GetServiceThen<T>(this AppServiceProvider self, Action<T> action)
        where T : class
    {
        T? service = self.GetService<T>();
        CallChainState state;

        if (service is not null)
        {
            action(service);
            state = CallChainState.SkipElse;
        }
        else
        {
            state = CallChainState.Continue;
        }

        return new CallChain<T?>(service, state);
    }
}

namespace HM.AppComponents.AppService;

public static class AppServiceProviderExtensions
{
    public static void GetServiceThen<T>(this AppServiceProvider self, Action<T> action)
        where T : class
    {
        T? service = self.GetService<T>();

        if (service is not null)
        {
            action(service);
        }
    }
}

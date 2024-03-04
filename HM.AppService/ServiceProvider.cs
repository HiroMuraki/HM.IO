namespace HM.AppService;

public class ServiceProvider : IServiceProvider
{
    public void RegisterService<T>(T service)
        where T : class
    {
        if (_services.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Unable to register service `{typeof(T)}` because it's already registered.");
        }

        _services[typeof(T)] = service;
    }

    public void UnregisterSerivce<T>()
    {
        _services.Remove(typeof(T));
    }

    public bool TryGetService<T>(out T? service)
        where T : class
    {
        return (service = GetService<T>()) is not null;
    }

    public T? GetService<T>()
        where T : class
    {
        return GetService(typeof(T)) as T;
    }

    public object? GetService(Type serviceType)
    {
        if (_services.TryGetValue(serviceType, out object? service))
        {
            return service;
        }
        else
        {
            throw new ServiceNotFoundException(serviceType);
        }
    }

    #region NonPublic
    private readonly Dictionary<Type, object> _services = [];
    #endregion
}

using System.Diagnostics.CodeAnalysis;

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

    public void UnregisterService<T>()
    {
        _services.Remove(typeof(T));
    }

    public Boolean TryGetService<T>([NotNullWhen(true)] out T? service)
        where T : class
    {
        service = GetServiceHelper(typeof(T), errorIfNotFound: false) as T;

        return service is not null;
    }

    public T GetService<T>()
        where T : class
    {
        return (T)GetService(typeof(T));
    }

    public Object GetService(Type serviceType)
    {
        return GetServiceHelper(serviceType, errorIfNotFound: true)!;
    }

    #region NonPublic
    private readonly Dictionary<Type, Object> _services = [];
    private Object? GetServiceHelper(Type serviceType, Boolean errorIfNotFound)
    {
        if (_services.TryGetValue(serviceType, out Object? service))
        {
            return service;
        }
        else if (errorIfNotFound)
        {
            throw new ServiceNotFoundException(serviceType);
        }
        else
        {
            return null;
        }
    }
    #endregion
}

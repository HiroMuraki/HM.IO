using HM.Common;
using System.Diagnostics.CodeAnalysis;

namespace HM.AppComponents.AppService;

public class AppServiceProvider :
    IServiceProvider
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

    public Option<T> GetService<T>()
        where T : class
    {
        return GetServiceHelper(typeof(T), errorIfNotFound: false) as T;
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
            throw new AppServiceNotFoundException(serviceType);
        }
        else
        {
            return null;
        }
    }
    #endregion
}

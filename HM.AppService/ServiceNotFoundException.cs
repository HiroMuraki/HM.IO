namespace HM.AppService;

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(Type service) : base($"Service `{service}` not found")
    {

    }
}

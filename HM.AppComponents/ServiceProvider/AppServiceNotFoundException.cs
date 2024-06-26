﻿namespace HM.AppComponents.AppService;

public class AppServiceNotFoundException : Exception
{
    public AppServiceNotFoundException(Type service) : base($"Service `{service}` not found")
    {

    }
}

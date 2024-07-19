using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ToDoList.Web.Configs;
using ToDoList.Web.Enums;

namespace ToDoList.Web.Services;

public abstract class ServiceBase
{
    protected readonly string serverHost;
    public ServiceBase(IOptions<ConfigBase> options)
    {
        serverHost = options.Value.Url;
    }

    protected abstract ControllerNames ControllerName { get; }

    protected string GetApiPoint(string routeSegment = null)
        => !string.IsNullOrEmpty(routeSegment)
        ? $"{serverHost}/{ControllerName.ToString()}/{routeSegment}"
        : $"{serverHost}/{ControllerName.ToString()}";
}

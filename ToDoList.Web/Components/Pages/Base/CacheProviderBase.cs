using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace ToDoList.Web.Components.Pages.Base;

public class CacheProviderBase : ComponentBaseClass
{
    [Inject]
    public IDistributedCache CacheProvider {  get; set; }

    [Inject]
    public IHttpContextAccessor HttpContext {  get; set; }

    [Inject]
    public IConfiguration ConfigProvider { get; set; }

    protected async Task SetStringInfoToCache(string key, string value)
    {
        if(IsCachePong())
            await CacheProvider.SetStringAsync(key, value);
    }

    protected async Task<string> GetStringFromCache(string key)
    {
        if (IsCachePong())
            return await CacheProvider.GetStringAsync(key);
        else
            return string.Empty;
    }

    protected string UserInfo { get; set; }

    private bool IsCachePong()
    {
        var cacheHost = ConfigProvider.GetSection("RedisCache:Host");
        var cachePort = ConfigProvider.GetSection("RedisCache:Port");
        var cacheInfo = $"{cacheHost.Value}:{cachePort.Value}";

        var connect = ConnectionMultiplexer.Connect(cacheInfo);
        if (connect != null && connect.IsConnected)
            return true;
        else
            return false;
    }
}

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.ResponseCompression;
using System.ComponentModel.Design;
using ToDoList.Web.Services;

namespace ToDoList.Web.Configs;

public static class ServicesConfig
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        // Connect mapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        services.Configure<ConfigBase>(config.GetSection("ServerHost"));

        // Cache
        services.AddDistributedMemoryCache();

        // Sessions
        services.AddSession();

        services.AddRazorPages();
        services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

        // Регистрация HttpClient 
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/octet-stream" });
        });

        // Adding HttpClient
        services.AddHttpClient("todo_list_http_client").ConfigureHttpClient(x =>
        {
            x.Timeout = TimeSpan.FromMinutes(5);
        });

        // Добавлен сервис для доступа к контексту запроса
        services.AddHttpContextAccessor();

        // Storage
        services.AddTransient<ProtectedSessionStorage>();

        // Project's API
        services.AddTransient<ToDoItemService>();
        services.AddTransient<PriorityService>();
        services.AddTransient<UserService>();

        // HTTP
        services.AddTransient<HttpService>();
    }
}

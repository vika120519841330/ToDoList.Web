using Microsoft.Extensions.Configuration;
using ToDoList.Server.Interfaces.Mq;
using ToDoList.Server.Interfaces.MQ;
using ToDoList.Server.Services.Mq;
using ToDoList.Server.Services.REST;

namespace ToDoList.Server.Configs;

public static class ServiceCollectionRegistry
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<PriorityService>();
        services.AddTransient<UserService>();
        services.AddTransient<ToDoItemService>();
        services.Configure<QueueConfigs>(configuration.GetSection("RabbitMQConfigs"));
    }
}
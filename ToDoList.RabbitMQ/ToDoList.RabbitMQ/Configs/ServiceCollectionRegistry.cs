using ToDoList.RabbitMQ.Interfaces;
using ToDoList.RabbitMQ.Services;

namespace ToDoList.RabbitMQ.Configs;

public static class ServiceCollectionRegistry
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueConfigs>(configuration.GetSection("RabbitMQConfigs"));
        services.AddTransient<IToDoItemMqServiceProducer, ToDoItemMqServiceProducer>();
    }
}

using ToDoList.Server.Services;

namespace ToDoList.Server.Configs;

public static class ServicesConfig
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<PriorityService>();
        services.AddTransient<UserService>();
        services.AddTransient<ToDoItemService>();
    }
}
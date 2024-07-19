using Microsoft.VisualBasic.FileIO;
using ToDoList.Server.Data.Models.DBModels;
using ToDoList.Repositories;
using ToDoList.Interfaces;

namespace ToDoList.Server.Configs;

public static class RepositoryConfig
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ITRepository<ToDoItem>, TRepository<ToDoItem>>();
        services.AddTransient<ITRepository<Priority>, TRepository<Priority>>();
        services.AddTransient<ITRepository<User>, TRepository<User>>();
    }
}
using Microsoft.VisualBasic.FileIO;
using ToDoList.Interfaces;
using ToDoList.Server.Data.Models.DBModels;

namespace ToDoList.Server.Repositories;

public class AppRepositories
{
    private readonly IServiceProvider serviceProvider;

    public AppRepositories(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public ITRepository<Priority> Priorities => serviceProvider.GetService<ITRepository<Priority>>();

    public ITRepository<User> Users => serviceProvider.GetService<ITRepository<User>>();

    public ITRepository<ToDoItem> ToDoItems => serviceProvider.GetService<ITRepository<ToDoItem>>();
}

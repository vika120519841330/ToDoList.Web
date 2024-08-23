using Microsoft.VisualBasic.FileIO;
using ToDoList.Server.Data.Models.DBModels;
using ToDoList.Server.Interfaces.REST;

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

using AutoMapper;
using ToDoList.Interfaces;
using ToDoList.Server.Data.Models.DBModels;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Repositories;

namespace ToDoList.Server.Services;

public class ToDoItemService : ServiceBase
{
    public ToDoItemService(AppRepositories appRepositories, IMapper mapper) : base(appRepositories, mapper) { }

    private List<Priority> prioritiesDb = new();
    private List<User> usersDb = new();
    private List<ToDoItem> todoListDb = new();

    private async Task InitPriorities(CancellationToken token = default) => prioritiesDb = await appRepositories.Priorities.GetAsync(token);
    private async Task InitUsers(CancellationToken token = default) => usersDb = await appRepositories.Users.GetAsync(token);
    private async Task InitTodoList(CancellationToken token = default) => todoListDb = await appRepositories.ToDoItems.GetAsync(token);
    public async Task<List<ToDoItemResponse>> GetToDoItemList(CancellationToken token = default)
    {
        await Task.WhenAll(
            new List<Task>
            {
                InitPriorities(token),
                InitUsers(token),
                InitTodoList(token),
            });

        var result = from todoitem in todoListDb
                     join user in usersDb.DefaultIfEmpty() on todoitem.UserId equals user.Id
                     join priority in prioritiesDb.DefaultIfEmpty() on todoitem.PriorityId equals priority.Id
                     select new ToDoItemResponse
                     {
                         Id = todoitem.Id,
                         Title = todoitem.Title,
                         Description = todoitem.Description,
                         IsCompleted = todoitem.IsCompleted,
                         DueDate = todoitem.DueDate,
                         PriorityId = todoitem.PriorityId,
                         PriorityValue = priority.Level,
                         UserId = todoitem.UserId,
                         UserName = user.Name,
                     };

        return result?.ToList() ?? new();
    }
}

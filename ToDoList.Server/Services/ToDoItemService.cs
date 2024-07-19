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

    public async Task<ToDoItemRequest> CreateToDoItem(ToDoItemRequest item, CancellationToken token = default)
    {
        if (item == null)
        {
            Message = "Задача не может быть добавлена в систему - она пустая";
            return null;
        }

        var itemToAdd = mapper.Map<ToDoItem>(item);
        var service = appRepositories.ToDoItems;
        var addedItem = await service.CreateAsync(itemToAdd, token);
        Message = service?.Message ?? string.Empty;

        if (addedItem == null) return null;

        return mapper.Map<ToDoItemRequest>(addedItem);
    }

    public async Task<ToDoItemRequest> UpdateToDoItem(ToDoItemRequest item, CancellationToken token = default)
    {
        if (item == null)
        {
            Message = "Задача не может быть обновлена в системе - она пустая";
            return null;
        }

        var itemToAdd = mapper.Map<ToDoItem>(item); 
        var service = appRepositories.ToDoItems;
        var updatedItem = await service.UpdateAsync(itemToAdd, token);
        Message = service?.Message ?? string.Empty;

        if (updatedItem == null) return null;

        return mapper.Map<ToDoItemRequest>(updatedItem);
    }

    public async Task<bool> RemoveToDoItem(int itemId, CancellationToken token = default)
    {
        if (itemId == 0)
        {
            Message = "Задача не может быть удалена из системы - идентификатор пустой";
            return false;
        }

        var service = appRepositories.ToDoItems;
        var result = await service.RemoveAsync(itemId, token);
        Message = service?.Message ?? string.Empty;

        return result;
    }
}

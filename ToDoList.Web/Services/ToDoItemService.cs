using Microsoft.Extensions.Options;
using ToDoList.Web.Configs;
using ToDoList.Web.Data.DTO;
using ToDoList.Web.Enums;

namespace ToDoList.Web.Services;

public class ToDoItemService : ServiceBase
{
    private readonly HttpService httpService;

    public ToDoItemService(HttpService httpService, IOptions<ConfigBase> options) : base(options)
    {
        this.httpService = httpService;
    }

    protected override ControllerNames ControllerName => ControllerNames.toDoItem;

    public async Task<List<ToDoItemFullModel>> GetListAsync(CancellationToken token = default)
        => await httpService.SendRequestAsync<List<ToDoItemFullModel>>(GetApiPoint(), HttpMethod.Get, token);

    public async Task<ToDoItemModel> UpdateAsync(ToDoItemModel item, CancellationToken token = default)
        => await httpService.SendRequestAsync<ToDoItemModel, ToDoItemModel>(GetApiPoint(), HttpMethod.Put, item, token);

    public async Task<ToDoItemModel> CreteAsync(ToDoItemModel item, CancellationToken token = default)
        => await httpService.SendRequestAsync<ToDoItemModel, ToDoItemModel>(GetApiPoint(), HttpMethod.Post, item, token);

    public async Task<bool> DeleteAsync(int itemId, CancellationToken token = default)
        => await httpService.SendRequestAsync<bool>(GetApiPoint(itemId.ToString()), HttpMethod.Delete, token);
}

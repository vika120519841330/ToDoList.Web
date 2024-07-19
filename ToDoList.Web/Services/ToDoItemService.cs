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
}

using Microsoft.Extensions.Options;
using ToDoList.Web.Configs;
using ToDoList.Web.Data.DTO;
using ToDoList.Web.Enums;

namespace ToDoList.Web.Services;

public class PriorityService : ServiceBase
{
    public PriorityService(HttpService httpService, IOptions<ConfigBase> options) : base(httpService, options) { }

    protected override ControllerNames ControllerName => ControllerNames.priority;

    public async Task<List<PriorityModel>> GetListAsync(CancellationToken token = default)
        => await httpService.SendRequestAsync<List<PriorityModel>>(GetApiPoint(), HttpMethod.Get, token);
}
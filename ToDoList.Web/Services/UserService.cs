using Microsoft.Extensions.Options;
using ToDoList.Web.Configs;
using ToDoList.Web.Data.DTO;
using ToDoList.Web.Enums;

namespace ToDoList.Web.Services;

public class UserService : ServiceBase
{
    public UserService(HttpService httpService, IOptions<ConfigBase> options) : base(httpService, options) { }

    protected override ControllerNames ControllerName => ControllerNames.user;

    public async Task<List<UserModel>> GetListAsync(CancellationToken token = default)
        => await httpService.SendRequestAsync<List<UserModel>>(GetApiPoint(), HttpMethod.Get, token);
}

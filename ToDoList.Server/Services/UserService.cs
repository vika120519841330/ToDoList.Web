using AutoMapper;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Repositories;

namespace ToDoList.Server.Services;

public class UserService : ServiceBase
{
    public UserService(AppRepositories appRepositories, IMapper mapper) : base(appRepositories, mapper) { }

    public async Task<List<UserResponse>> GetUsers(CancellationToken token = default)
    {
        var users = await appRepositories.Users.GetAsync(token);

        if (users.Any())
        {
            return users.Select(item => mapper.Map<UserResponse>(item)).ToList();
        }
        else return new();
    }
}

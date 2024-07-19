using AutoMapper;
using ToDoList.Interfaces;
using ToDoList.Server.Data.Models.DBModels;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Repositories;

namespace ToDoList.Server.Services;

public class PriorityService : ServiceBase
{
    public PriorityService(AppRepositories appRepositories, IMapper mapper) : base(appRepositories, mapper) { }

    public async Task<List<PriorityResponse>> GetPriorities(CancellationToken token = default)
    {
        var priorities = await appRepositories.Priorities.GetAsync(token);

        if (priorities.Any())
        {
            return priorities.Select(item => mapper.Map<PriorityResponse>(item)).ToList();
        }
        else return new();
    }
}

using AutoMapper;
using ToDoList.Server.Repositories;

namespace ToDoList.Server.Services;

public abstract class ServiceBase
{
    protected readonly IMapper mapper;
    protected readonly AppRepositories appRepositories;
    public ServiceBase(AppRepositories appRepositories, IMapper mapper)
    {
        this.appRepositories = appRepositories;
        this.mapper = mapper;
    }
}

using AutoMapper;
using ToDoList.Interfaces;
using ToDoList.Server.Repositories;

namespace ToDoList.Server.Services.REST;

public abstract class ServiceBase : INotify
{
    protected readonly IMapper mapper;
    protected readonly AppRepositories appRepositories;
    public ServiceBase(AppRepositories appRepositories, IMapper mapper)
    {
        this.appRepositories = appRepositories;
        this.mapper = mapper;
    }

    public string Message { get; set; } = string.Empty;
}

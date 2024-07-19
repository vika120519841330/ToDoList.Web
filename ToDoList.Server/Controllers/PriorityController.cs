using Microsoft.AspNetCore.Mvc;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Services;

namespace ToDoList.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PriorityController : ControllerBase
{
    private readonly PriorityService priorityService;
    public PriorityController(PriorityService priorityService)
    {
        this.priorityService = priorityService;
    }

    [HttpGet]
    public async Task<IEnumerable<PriorityResponse>> Get([FromHeader] CancellationToken token = default)
        => await priorityService.GetPriorities(token);
}

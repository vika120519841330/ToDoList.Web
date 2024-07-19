using Microsoft.AspNetCore.Mvc;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Services;

namespace ToDoList.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoItemController : ControllerBase
    {
        private readonly ToDoItemService toDoItemService;
        public ToDoItemController(ToDoItemService toDoItemService)
        {
            this.toDoItemService = toDoItemService;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoItemResponse>> Get([FromHeader]CancellationToken token = default)
            => await toDoItemService.GetToDoItemList(token);
    }
}

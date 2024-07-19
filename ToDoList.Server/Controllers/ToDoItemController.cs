using Microsoft.AspNetCore.Mvc;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Services;

namespace ToDoList.Server.Controllers;

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
    [ProducesResponseType(typeof(IEnumerable<ToDoItemRequest>), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<ToDoItemResponse>> Get([FromHeader]CancellationToken token = default)
        => await toDoItemService.GetToDoItemList(token);

    [HttpPost]
    [ProducesResponseType(typeof(ActionResult), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] ToDoItemRequest item, [FromHeader] CancellationToken token = default)
    {
        var result = await toDoItemService.CreateToDoItem(item, token);
        if (result == null)
            return BadRequest(toDoItemService?.Message ?? "Ошибка при добавлении ");
        else
            return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ActionResult), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] ToDoItemRequest item, [FromHeader] CancellationToken token = default)
    {
        var result = await toDoItemService.UpdateToDoItem(item, token);
        if (result == null)
            return BadRequest(toDoItemService?.Message ?? "Ошибка при обновлении ");
        else
            return Ok(result);
    }

    [HttpDelete("{itemId}")]
    [ProducesResponseType(typeof(ActionResult), 200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Remove([FromRoute] int itemId, [FromHeader] CancellationToken token = default)
    {
        var result = await toDoItemService.RemoveToDoItem(itemId, token);
        if (!result)
            return BadRequest(toDoItemService?.Message ?? "Ошибка при удалении ");
        else
            return Ok(result);
    }
}

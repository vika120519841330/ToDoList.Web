using Microsoft.AspNetCore.Mvc;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Services;

namespace ToDoList.Server.Controllers;

public class UserController : ControllerBase
{
    private readonly UserService userService;
    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<UserResponse>> Get([FromHeader] CancellationToken token = default)
        => await userService.GetUsers(token);
}
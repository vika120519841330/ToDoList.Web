using Microsoft.AspNetCore.Mvc;
using ToDoList.RabbitMQ.Interfaces;
using ToDoList.RabbitMQ.Services;

namespace ToDoList.RabbitMQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoItemsMQController : ControllerBase
    {
        private readonly IToDoItemMqService mqService;

        public ToDoItemsMQController(IToDoItemMqService mqService)
        {
            this.mqService = mqService;
        }

        [Route("[action]/{message}")]
        [HttpGet]
        public IActionResult SendMessage(string message)
        {
            mqService.SendMessage(message);

            return Ok("Message where send.");
        }
    }
}

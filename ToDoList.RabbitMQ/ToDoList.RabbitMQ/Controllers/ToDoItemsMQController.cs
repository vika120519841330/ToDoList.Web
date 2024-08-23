using Microsoft.AspNetCore.Mvc;
using ToDoList.RabbitMQ.Interfaces;
using ToDoList.RabbitMQ.MQObjects;
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
        public async Task<IActionResult> SendMessage(string message)
        {
            var messageToSend = new MQMessage(message);
            var result = await  mqService.SendMessage(messageToSend);

            if (result)
                return Ok($"Sending seccess. Details message: {messageToSend.GetString()}. ");
            else
                return StatusCode(503, "Sending failed. ");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]object mqObject)
        {
            var messageToSend = new MQObject(mqObject);
            var result = await mqService.SendMessage(messageToSend);

            if (result)
                return Ok($"Sending seccess. Details message: {messageToSend.GetString()}. ");
            else
                return StatusCode(503, "Sending failed. ");
        }
    }
}
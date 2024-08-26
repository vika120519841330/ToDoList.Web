using Microsoft.Extensions.Options;
using System.Text.Json;
using ToDoList.Server.Configs;
using ToDoList.Server.Data.Models.DBModels;
using ToDoList.Server.Data.Models.DTO.Response;
using ToDoList.Server.Enums;
using ToDoList.Server.Interfaces.Mq;

namespace ToDoList.Server.Services.Mq
{
    public class ToDoItemMqServiceConsumer : MqConsumerServiceBase, IToDoItemMqServiceConsumer
    {
        public ToDoItemMqServiceConsumer(IOptions<QueueConfigs> options) : base(options) { }

        protected override QueueConfig QueueConfig
            => options.Value.Queues.FirstOrDefault(config => config.Name.Equals(QueuesNames.ToDoitemQueue.ToString()));

        protected override async Task ProcessContent(byte[] content, CancellationToken token)
        {
            using var ms = new MemoryStream(content);
            var item = await JsonSerializer.DeserializeAsync<ToDoItemRequest>(ms, SerializersOptions, token);

            // TODO:

            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Received message: «{item}»");
        }
    }
}

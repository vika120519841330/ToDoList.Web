using Microsoft.Extensions.Options;
using ToDoList.Server.Configs;
using ToDoList.Server.Enums;
using ToDoList.Server.Interfaces.Mq;

namespace ToDoList.Server.Services.Mq
{
    public class ToDoItemMqServiceConsumer : MqConsumerServiceBase, IToDoItemMqServiceConsumer
    {
    private readonly string hostName;

        public ToDoItemMqServiceConsumer(IOptions<QueueConfigs> options) : base(options) { }

        protected override QueueConfig QueueConfig
            => options.Value.Queues.FirstOrDefault(config => config.Name.Equals(QueuesNames.ToDoitemQueue.ToString()));

        protected override Task ProcessContent(byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}

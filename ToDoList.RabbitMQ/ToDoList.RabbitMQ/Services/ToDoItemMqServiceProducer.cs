using Microsoft.Extensions.Options;
using ToDoList.RabbitMQ.Configs;
using ToDoList.RabbitMQ.Enums;
using ToDoList.RabbitMQ.Interfaces;

namespace ToDoList.RabbitMQ.Services;

public class ToDoItemMqServiceProducer : MqProducerServiceBase, IToDoItemMqServiceProducer
{
    private readonly string hostName;

    public ToDoItemMqServiceProducer(IOptions<QueueConfigs> options) : base(options) { }

    protected override QueueConfig QueueConfig
        => options.Value.Queues.FirstOrDefault(config => config.Name.Equals(QueuesNames.ToDoitemQueue.ToString()));
}

using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ToDoList.RabbitMQ.Interfaces;
using Microsoft.Extensions.Options;
using ToDoList.RabbitMQ.Configs;
using ToDoList.RabbitMQ.Enums;

namespace ToDoList.RabbitMQ.Services;

public class ToDoItemMqService : RabbitMqServiceBase, IToDoItemMqService
{
    private readonly string hostName;

    public ToDoItemMqService(IOptions<QueueConfigs> options) : base(options) { }

    protected override QueueConfig QueueConfig
        => options.Value.Queues.FirstOrDefault(config => config.Name.Equals(QueuesNames.ToDoitemQueue.ToString()));
}

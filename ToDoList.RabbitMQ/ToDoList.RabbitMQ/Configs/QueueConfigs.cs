using ToDoList.RabbitMQ.Configs;

namespace ToDoList.RabbitMQ.Configs;

public class QueueConfigs : ConfigBase
{
    public IEnumerable<QueueConfig> Queues { get; set; } = new List<QueueConfig>();
}

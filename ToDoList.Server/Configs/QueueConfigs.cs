namespace ToDoList.Server.Configs;

public class QueueConfigs : ConfigBase
{
    public IEnumerable<QueueConfig> Queues { get; set; } = new List<QueueConfig>();
}

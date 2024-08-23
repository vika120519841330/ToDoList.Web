namespace ToDoList.RabbitMQ.Interfaces;

public interface IRabbitMqServiceBase
{
    Task<bool> SendMessage<T>(T obj, CancellationToken token = default) where T : IMQBase;
}

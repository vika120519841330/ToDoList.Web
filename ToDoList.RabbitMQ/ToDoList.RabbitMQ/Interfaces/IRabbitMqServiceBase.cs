namespace ToDoList.RabbitMQ.Interfaces;

public interface IRabbitMqServiceBase
{
    Task<bool> SendMessageAsync<T>(T obj, CancellationToken token = default) where T : IMQBase;
}

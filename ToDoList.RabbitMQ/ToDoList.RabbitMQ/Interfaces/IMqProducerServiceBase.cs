namespace ToDoList.RabbitMQ.Interfaces;

public interface IMqProducerServiceBase
{
    Task<bool> SendMessage<T>(T obj, CancellationToken token = default) where T : IMQBase;
}

namespace ToDoList.RabbitMQ.Interfaces;

public interface IRabbitMqServiceBase
{
    bool SendMessage<T>(T obj) where T : IMQBase;
}

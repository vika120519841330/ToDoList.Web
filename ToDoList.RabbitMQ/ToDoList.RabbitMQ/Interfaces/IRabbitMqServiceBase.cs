namespace ToDoList.RabbitMQ.Interfaces;

public interface IRabbitMqServiceBase
{
    void SendMessage(object obj);

    void SendMessage(string message);
}

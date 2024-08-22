using ToDoList.RabbitMQ.Interfaces;

namespace ToDoList.RabbitMQ.MQObjects;

public class MQMessage : IMQBase
{
    public MQMessage() { }
    public MQMessage(string message) => Message = message ?? string.Empty;
    public string Message { get; set; }
    public string GetString() => Message;
}

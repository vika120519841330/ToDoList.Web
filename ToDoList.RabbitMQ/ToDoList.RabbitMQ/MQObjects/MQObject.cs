using ToDoList.RabbitMQ.Interfaces;

namespace ToDoList.RabbitMQ.MQObjects;

public class MQObject : IMQBase
{
    public MQObject() { }

    public MQObject(object mqObjectItem) => MqObjectItem = mqObjectItem;

    public object MqObjectItem { get; set; }

    public string GetString() => MqObjectItem.ToString();
}

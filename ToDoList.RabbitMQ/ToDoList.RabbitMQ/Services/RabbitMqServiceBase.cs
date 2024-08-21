using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ToDoList.RabbitMQ.Configs;
using ToDoList.RabbitMQ.Interfaces;

namespace ToDoList.RabbitMQ.Services;

public abstract class RabbitMqServiceBase : IRabbitMqServiceBase
{
    private readonly string hostName;
    protected readonly IOptions<QueueConfigs> options;
    public RabbitMqServiceBase(IOptions<QueueConfigs> options)
    {
        this.options = options;
        hostName = options.Value.Host;
    }

    protected abstract QueueConfig QueueConfig { get; }

    protected virtual bool Durable => true;

    protected virtual bool Exclusive => false;

    protected virtual bool AutoDelete => false;

    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = Protocols.DefaultProtocol.DefaultPort,
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
        };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var queueName = QueueConfig.Name;

        channel.QueueDeclare(queue: queueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: null);

        var body = Encoding.UTF8.GetBytes(message ?? "");

        channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
    }
}

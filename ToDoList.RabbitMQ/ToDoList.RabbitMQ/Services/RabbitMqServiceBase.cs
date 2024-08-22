using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ToDoList.RabbitMQ.Configs;
using ToDoList.RabbitMQ.Interfaces;
using System.Text.Json.Serialization.Metadata;
using System.IO;

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

    private static string FileName => "message_to_send.json";

    private static string FilePath = Path.Combine("./wwwroot/temp", FileName);

    private static ConnectionFactory MQConnectionFactory = new ConnectionFactory()
    {
        HostName = "localhost",
        Port = Protocols.DefaultProtocol.DefaultPort,
        UserName = "guest",
        Password = "guest",
        VirtualHost = "/",
        ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
    };

    protected abstract QueueConfig QueueConfig { get; }

    private string QueueName => QueueConfig?.Name ?? string.Empty;

    protected virtual bool Durable => true;

    protected virtual bool Exclusive => false;

    protected virtual bool AutoDelete => false;

    public async Task<bool> SendMessageAsync<T>(T obj, CancellationToken token = default)
        where T : IMQBase
    {
        using (var stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
        {
            await JsonSerializer.SerializeAsync<T>(utf8Json: stream, value: obj, cancellationToken: token);
        }
        
        return SendMessage();
    }

    private bool SendMessage()
    {
        if (!File.Exists(FilePath)) return false;

        using var connection = MQConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var queueName = QueueConfig.Name;

        channel.QueueDeclare(queue: queueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: null);

        using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new BinaryReader(stream);
        var body = reader.ReadBytes((Int32)(new FileInfo(FilePath).Length));

        channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);

        return true;
    }
}

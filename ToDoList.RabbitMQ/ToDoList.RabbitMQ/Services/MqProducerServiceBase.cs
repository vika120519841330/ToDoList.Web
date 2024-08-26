using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ToDoList.RabbitMQ.Configs;
using System.Text.Json.Serialization.Metadata;
using System.IO;
using ToDoList.RabbitMQ.Interfaces;
using System.Text.Json.Serialization;

namespace ToDoList.RabbitMQ.Services;

public abstract class MqProducerServiceBase : IMqProducerServiceBase
{
    private readonly string hostName;
    protected readonly IOptions<QueueConfigs> options;

    public MqProducerServiceBase(IOptions<QueueConfigs> options)
    {
        this.options = options;
        hostName = options.Value.Host;
    }

    private static JsonSerializerOptions SerializersOptions
    => new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = false,
        IncludeFields = false,
        IgnoreReadOnlyFields = true,
        IgnoreReadOnlyProperties = false,
        MaxDepth = 3,
        DefaultBufferSize = 32 * 1024 * 1024, // 16Kb default - up to 32Mb
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
    };

    private static ConnectionFactory MQConnectionFactory = new ConnectionFactory()
    {
        HostName = "localhost",
        Port = Protocols.DefaultProtocol.DefaultPort,
        UserName = "guest",
        Password = "guest",
        VirtualHost = "/",
        ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
    };

    private byte[] Data {  get; set; }

    protected abstract QueueConfig QueueConfig { get; }

    private string QueueName => QueueConfig?.Name ?? string.Empty;

    protected virtual bool Durable => true;

    protected virtual bool Exclusive => false;

    protected virtual bool AutoDelete => false;

    public async Task<bool> SendMessage<T>(T obj, CancellationToken token = default)
        where T : IMQBase
        => await await Task.Factory.StartNew(() => Data = JsonSerializer.SerializeToUtf8Bytes<T>(value: obj, options: SerializersOptions))
        .ContinueWith(async (t) => await SendMessage(token));

    private async Task<bool> SendMessage(CancellationToken token = default)
    {
        if ((Data?.Length ?? 0) == 0) return false;

        var tcs = new TaskCompletionSource<bool>(token);

        using var connection = MQConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: null);

        channel.BasicPublish(exchange: "",
                routingKey: QueueName,
                basicProperties: null,
                body: Data);

        tcs.SetResult(true);
        await tcs.Task;
        return true;
    }
}

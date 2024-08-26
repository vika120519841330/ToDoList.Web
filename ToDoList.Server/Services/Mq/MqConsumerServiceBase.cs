using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using ToDoList.Server.Configs;
using System.Text;
using System.Threading.Channels;
using ToDoList.Server.Interfaces.MQ;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace ToDoList.Server.Services.Mq;

public abstract class MqConsumerServiceBase : BackgroundService, IMqConsumerServiceBase
{
    private readonly string hostName;
    protected readonly IOptions<QueueConfigs> options;

    public MqConsumerServiceBase(IOptions<QueueConfigs> options)
    {
        this.options = options;
        hostName = options.Value.Host;
    }

    protected abstract QueueConfig QueueConfig { get; }

    private string QueueName => QueueConfig?.Name ?? string.Empty;

    protected virtual bool Durable => true;

    protected virtual bool Exclusive => false;

    protected virtual bool AutoDelete => false;

    private IConnection Connection {  get; set; }

    private IModel Channel { get; set; }

    private void InitMqConnection()
    {
        var factory = new ConnectionFactory
        {
            VirtualHost = "/",
            HostName = "localhost",
            Port = Protocols.DefaultProtocol.DefaultPort,
            UserName = "guest",
            Password = "guest",
            ContinuationTimeout = new TimeSpan(10, 0, 0, 0),
        };

        Connection = factory.CreateConnection();
    }

    private void InitMqChannel()
    {
        Channel = Connection.CreateModel();

        Channel.ExchangeDeclare(exchange: QueueName,
            type: ExchangeType.Direct,
            durable: Durable,
            autoDelete: AutoDelete,
            arguments: null);

        Channel.QueueDeclare(queue: QueueName,
            durable: Durable,
            exclusive: Exclusive,
            autoDelete: AutoDelete,
            null);

        Channel.QueueBind(queue: QueueName, exchange: QueueName, routingKey: QueueName, null);
    }

    protected static JsonSerializerOptions SerializersOptions
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

    protected abstract Task ProcessContent(byte[] content, CancellationToken token);

    private string ReceiveIndividualMessage()
    {
        string originalMessage = string.Empty;
        InitMqConnection();
        InitMqChannel();
        var result = Channel.BasicGet(QueueName, false);
        if (result != null)
        {
            var body = result.Body.ToArray();
            originalMessage = Encoding.UTF8.GetString(body);
        }
        return originalMessage;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var action = async (byte[] data) => await ProcessContent(data, stoppingToken);

        InitMqConnection();
        InitMqChannel();

        var consumer = new EventingBasicConsumer(Channel);
        consumer.Received += async (ch, ea) =>
        {
            if (ea != null && !ea.Body.IsEmpty)
            {
                await action.Invoke(ea?.Body.ToArray());
            }

            Channel.BasicAck(ea.DeliveryTag, false);
        };

        var consumerTag = Channel.BasicConsume(QueueName, true, consumer);
    }

    public override void Dispose()
    {
        Channel.Dispose();
        Connection.Dispose();
        base.Dispose();
    }
}

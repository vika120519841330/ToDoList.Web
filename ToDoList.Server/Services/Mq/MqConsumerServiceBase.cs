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

    private IConnection GetMqConnection()
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

        var connectionMq = factory.CreateConnection();

        return connectionMq;
    }

    private IModel GetMqChannel(IConnection connection)
    {
        IModel model = connection.CreateModel();

        model.ExchangeDeclare(exchange: QueueName,
            type: ExchangeType.Direct,
            durable: Durable,
            autoDelete: AutoDelete,
            arguments: null);

        model.QueueDeclare(queue: QueueName,
            durable: Durable,
            exclusive: Exclusive,
            autoDelete: AutoDelete,
            null);

        model.QueueBind(queue: QueueName, exchange: QueueName, routingKey: QueueName, null);

        return model;
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
        using var connection = GetMqConnection();
        using var channel = GetMqChannel(connection);
        var result = channel.BasicGet(QueueName, false);
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

        using var connection = GetMqConnection();
        using var channel = GetMqChannel(connection);

        //var subscription = new Subscription(model, queueName, false);
        //while (true)
        //{
        //    BasicDeliverEventArgs basicDeliveryEventArgs = subscription.Next();
        //    string messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
        //    messagesTextBox.Invoke((MethodInvoker)delegate { messagesTextBox.Text += messageContent + "\r\n"; });
        //    subscription.Ack(basicDeliveryEventArgs);
        //}

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
            if (ea != null && !ea.Body.IsEmpty) return;
                await ProcessContent(ea?.Body.ToArray(), stoppingToken);

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(QueueName, true, consumer);
    }
}

using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using ToDoList.Server.Configs;
using System.Text;
using System.Threading.Channels;
using ToDoList.Server.Interfaces.MQ;

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

    protected abstract Task ProcessContent(byte[] content);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        using var connection = MQConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var queue = channel.QueueDeclare(queue: QueueName, 
            durable: Durable,
            exclusive: Exclusive,
            autoDelete: AutoDelete,
            arguments: null);

        if (queue == null) return;

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
            if (ea != null && !ea.Body.IsEmpty) return;
                await ProcessContent(ea?.Body.ToArray());

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(QueueName, true, consumer);
    }
}

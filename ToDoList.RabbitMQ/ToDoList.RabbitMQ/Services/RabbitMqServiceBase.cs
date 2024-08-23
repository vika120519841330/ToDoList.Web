﻿using Microsoft.Extensions.Options;
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
        => await await Task.Factory.StartNew(() => Data = JsonSerializer.SerializeToUtf8Bytes<T>(obj), token)
        .ContinueWith(async (t) => await SendMessage());

    private async Task<bool> SendMessage()
    {
        if ((Data?.Length ?? 0) == 0) return false;

        var tcs = new TaskCompletionSource<bool>();
        using var connection = MQConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var queueName = QueueConfig.Name;

        channel.QueueDeclare(queue: queueName,
                durable: Durable,
                exclusive: Exclusive,
                autoDelete: AutoDelete,
                arguments: null);

        channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: Data);

        tcs.SetResult(true);
        await tcs.Task;
        return true;
    }
}

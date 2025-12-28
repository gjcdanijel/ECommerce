using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orders.API.Services;

public interface IMessageBus
{
    Task PublishAsync<T>(string queueName, T message);
    void Subscribe<T>(string queueName, Func<T, Task> handler);
}

public class RabbitMqMessageBus : IMessageBus, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _hostName;
    private bool _initialized;

    public RabbitMqMessageBus(IConfiguration configuration)
    {
        _hostName = configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost";
    }

    private async Task EnsureConnectionAsync()
    {
        if (_initialized) return;

        var factory = new ConnectionFactory { HostName = _hostName };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        _initialized = true;
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        await EnsureConnectionAsync();

        await _channel!.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);
    }

    public void Subscribe<T>(string queueName, Func<T, Task> handler)
    {
        Task.Run(async () =>
        {
            await EnsureConnectionAsync();

            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                if (message != null)
                {
                    await handler(message);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.CloseAsync();
        if (_connection != null) await _connection.CloseAsync();
    }
}

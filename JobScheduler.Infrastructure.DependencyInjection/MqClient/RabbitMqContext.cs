using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace JobScheduler.Infrastructure.DependencyInjection.MqClient;

/// <inheritdoc/>
public class RabbitMqContext : IRabbitMqContext
{
    private readonly ConnectionFactory _factory;
    private readonly RabbitMqIdentifier _options;
    private readonly IConnection _conn;
    private readonly ILogger<RabbitMqContext> _logger;

    /// <summary>
    /// Creates an instance of <see cref="RabbitMqContext"/>
    /// </summary>
    public RabbitMqContext(IOptions<RabbitMqIdentifier> options, ILogger<RabbitMqContext> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _factory = new ConnectionFactory() { Uri = new Uri(_options.ConnectionString), DispatchConsumersAsync = true };
        _conn = _factory.CreateConnection();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public void ProduceMessage<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        using var channel = _conn.CreateModel();

        channel.QueueDeclare(_options.QueueName, false, false, false);

        channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Fanout, durable: false);

        channel.QueueBind(
             queue: _options.QueueName,
             exchange: _options.ExchangeName,
             routingKey: typeof(T).FullName);

        channel.BasicPublish(
            exchange: _options.ExchangeName,
            routingKey: typeof(T).FullName,
            body: body
        );
    }

    /// <inheritdoc/>
    public void ConsumeMessage<T>(Func<T, Task> action)
    {
        using var channel = _conn.CreateModel();

        channel.QueueDeclare(_options.QueueName, false, false, false);

        channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Fanout);

        channel.QueueBind(
            queue: _options.QueueName,
            exchange: _options.ExchangeName,
            routingKey: typeof(T).FullName);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (model, eventArgs) =>
        {
            await ConsumerReceived(action, eventArgs);
        };

        channel.BasicConsume(
            queue: _options.QueueName,
            autoAck: true,
            consumer: consumer);
    }

    private async Task ConsumerReceived<T>(Func<T, Task> action, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

        if (message is not null)
        {
            await action(message);
        }
        else
        {
            _logger.LogError("Message returned from the queue cannot be null!");
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        try
        {
            _conn.Close();
            _conn.Dispose();

            _logger.LogInformation("Closing rabbitmq connection ...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
        }
    }
}

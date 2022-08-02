using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace JobScheduler.Infrastructure.DependencyInjection.MqClient;

/// <inheritdoc/>
public class RabbitMqClient : IRabbitMqClient
{
    private readonly ConnectionFactory _factory;
    private readonly RabbitMqIdentifier _options;
    private readonly IConnection _conn;
    private readonly ILogger<RabbitMqClient> _logger;

    /// <summary>
    /// Creates an instance of <see cref="RabbitMqClient"/>
    /// </summary>
    public RabbitMqClient(IOptions<RabbitMqIdentifier> options, ILogger<RabbitMqClient> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _factory = new ConnectionFactory()
        {
            Uri = new Uri(_options.ConnectionString),
            DispatchConsumersAsync = true
        };
        _conn = _factory.CreateConnection();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public Task ProduceMessage<T>(T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            using var channel = _conn.CreateModel();

            channel.QueueDeclare(_options.QueueName, false, false, false);

            channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Direct);

            channel.QueueBind(
                 queue: _options.QueueName,
                 exchange: _options.ExchangeName,
                 routingKey: typeof(T).Name);

            channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: typeof(T).Name,
                body: body
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ConsumeMessage<T>(Func<T, Task> action)
    {
        try
        {
            using var channel = _conn.CreateModel();
            channel.QueueDeclare(_options.QueueName, false, false, false);

            channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Direct);

            channel.QueueBind(
                queue: _options.QueueName,
                exchange: _options.ExchangeName,
                routingKey: typeof(T).Name);

            channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, eventArgs) =>
            {
                await ConsumerReceived(action, eventArgs);

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(
                queue: _options.QueueName,
                autoAck: false,
                consumer: consumer);

            channel.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);
        }

        return Task.CompletedTask;
    }

    private async Task ConsumerReceived<T>(Func<T, Task> action, BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var job = JsonSerializer.Deserialize<T>(message);

        if (job is not null)
        {
            await action(job);
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

using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Infrastructure.DependencyInjection.MqClient;

public class RabbitMqIdentifier
{
    [Required]
    public string ConnectionString { get; set; } = default!;

    [Required]
    public string QueueName { get; set; } = default!;

    [Required]
    public string ExchangeName { get; set; } = default!;
}

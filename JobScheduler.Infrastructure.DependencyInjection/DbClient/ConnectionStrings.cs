using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Infrastructure.DependencyInjection.DbClient;

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; } = default!;
}

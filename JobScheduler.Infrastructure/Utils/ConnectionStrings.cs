using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Infrastructure.Utils;

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; } = default!;
}

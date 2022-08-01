namespace JobScheduler.Infrastructure.Models;

public record JobDto(
    long JobId,
    string StartingTime,
    string? Duration,
    long Status,
    string Input,
    string? Output
);

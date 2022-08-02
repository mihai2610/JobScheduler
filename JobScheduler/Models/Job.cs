namespace JobScheduler.Models
{
    public record Job(
        long JobId,
        DateTime StartingTime,
        TimeSpan? Duration,
        JobStatusType Status,
        IReadOnlyCollection<int> Input,
        IReadOnlyCollection<int>? Output
    );
}

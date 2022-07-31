using System.Runtime.Serialization;

namespace JobScheduler.Models
{
    public record Job(
        long JobId,
        TimeSpan Timestamp,
        TimeSpan Duration,
        JobStatusType Status,
        IReadOnlyCollection<int> Input,
        IReadOnlyCollection<int> Output
    );
}

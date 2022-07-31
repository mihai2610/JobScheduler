namespace JobScheduler.Api.Models
{
    /// <summary>
    /// View model for a job
    /// </summary>
    /// <param name="JobId">A unique ID assigned by the application</param>
    /// <param name="Timestamp">When was the job enqueued</param>
    /// <param name="Duration">How much time did it take to execute the job</param>
    /// <param name="Status">Job status for example "pending" or "completed"</param>
    public record JobView(
        long JobId,
        TimeSpan Timestamp,
        TimeSpan Duration,
        JobStatusTypeView Status,
        IReadOnlyCollection<int> Input,
        IReadOnlyCollection<int> Output
    );
}

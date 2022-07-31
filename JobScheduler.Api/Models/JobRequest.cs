namespace JobScheduler.Api.Models
{
    /// <summary>
    /// Model to describe the job to be performed
    /// </summary>
    /// <param name="Input">Input of the job to be performed</param>
    public record JobRequest(
        IReadOnlyCollection<int> Input
    );
}

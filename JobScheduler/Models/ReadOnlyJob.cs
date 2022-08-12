namespace JobScheduler.Models;

/// <summary>
/// This model should be used only for fetching jobs information
/// </summary>
public class ReadOnlyJob : IJob<string, string?>
{
    public long JobId { get; set; }
    public DateTime StartingTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public JobStatusType Status { get; set; }
    public string Input { get; set; }
    public string? Output { get; set; }

    public Task<string?> Execute(string input)
    {
        throw new NotImplementedException();
    }
}

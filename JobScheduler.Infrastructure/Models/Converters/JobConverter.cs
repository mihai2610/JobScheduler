using JobScheduler.Models;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Models.Converters;

internal static class JobConverter
{
    public static Job ToModel(this JobDto job) => new(
        JobId: job.JobId,
        StartingTime: DateTime.Parse(job.StartingTime),
        Duration: job.Duration is null ? null : TimeSpan.Parse(job.Duration),
        Status: (JobStatusType)job.Status,
        Input: JsonSerializer.Deserialize<IReadOnlyCollection<int>>(job.Input) ?? new List<int>(),
        Output: job.Output is null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<int>>(job.Output)
    );

    public static IReadOnlyCollection<Job> ToModel(this IEnumerable<JobDto> jobs) =>
        jobs.Select(q => q.ToModel()).ToList();
}

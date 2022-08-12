using JobScheduler.Models;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Models.Converters;

internal static class JobConverter
{
    public static TJob ToModel<TJob, TInput, TOutput>(this JobDto job) where TJob : IJob<TInput, TOutput>, new() => new TJob()
    {
        JobId = job.JobId,
        StartingTime = DateTime.Parse(job.StartingTime),
        Duration = job.Duration is null ? null : TimeSpan.Parse(job.Duration),
        Status = (JobStatusType)job.Status,
        Input = job.Input is TInput input ? input : JsonSerializer.Deserialize<TInput>(job.Input) ?? throw new ArgumentNullException(nameof(job)),
        Output = job.Output is TOutput output ? output : (job.Output is null ? default : JsonSerializer.Deserialize<TOutput>(job.Output)) // to remove default
    };

    public static List<TJob> ToModel<TJob, TInput, TOutput>(this IEnumerable<JobDto> jobs) where TJob : IJob<TInput, TOutput>, new() =>
        jobs.Select(q => q.ToModel<TJob, TInput, TOutput>()).ToList();
}

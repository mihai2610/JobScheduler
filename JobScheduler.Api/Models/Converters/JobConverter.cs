using JobScheduler.Models;

namespace JobScheduler.Api.Models.Converters
{
    internal static class JobConverter
    {
        public static JobView ToView<I,O>(this IJob<I,O> job) => new(
            job.JobId,
            job.StartingTime,
            job.Duration,
            job.Status.ToString(),
            job.Input,
            job.Output
        );

        public static IReadOnlyCollection<JobView> ToView(this IEnumerable<ReadOnlyJob> jobs) =>
            jobs.Select(q => q.ToView()).ToList();
    }
}

using JobScheduler.Models;

namespace JobScheduler.Api.Models.Converters
{
    internal static class JobConverter
    {
        public static JobView ToView(this SortListJob job) => new(
            job.JobId,
            job.StartingTime,
            job.Duration,
            job.Status.ToString(),
            job.Input,
            job.Output
        );

        public static IReadOnlyCollection<JobView> ToView(this IEnumerable<SortListJob> jobs) =>
            jobs.Select(q => q.ToView()).ToList();
    }
}

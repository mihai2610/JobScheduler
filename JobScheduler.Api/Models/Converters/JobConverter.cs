using JobScheduler.Models;

namespace JobScheduler.Api.Models.Converters
{
    internal static class JobConverter
    {
        public static JobView ToView(this Job job) => new(
            job.JobId,
            job.Timestamp,
            job.Duration,
            (JobStatusTypeView)job.Status,
            job.Input,
            job.Output
        );

        public static IReadOnlyCollection<JobView> ToView(this IEnumerable<Job> jobs) =>
            jobs.Select(q => q.ToView()).ToList();
    }
}

using JobScheduler.Models;

namespace JobScheduler.Queries;

/// <summary>
/// Fteches a list of jobs
/// </summary>
public interface IGetAllJobsQuery
{
    Task<IReadOnlyCollection<Job>> Execute();
}

using JobScheduler.Models;

namespace JobScheduler.Queries;

/// <summary>
/// Fteches a list of jobs
/// </summary>
public interface IGetAllJobsQuery
{
    /// <summary>
    /// Fetches the list with all jobs
    /// </summary>
    /// <returns>List of jobs</returns>
    Task<IReadOnlyCollection<Job>> Execute();
}

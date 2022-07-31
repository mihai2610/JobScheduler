using JobScheduler.Models;

namespace JobScheduler.Queries;

/// <summary>
/// Fetches a job
/// </summary>
public interface IGetJobByIdQuery
{
    /// <summary>
    /// Method to fetch a job based on <paramref name="jobId"/>
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    Task<Job> Execute(long jobId);
}

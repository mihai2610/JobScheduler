using JobScheduler.Models;

namespace JobScheduler.Queries;

/// <summary>
/// Fteches a list of jobs
/// </summary>
public interface IGetAllJobsQuery
{
    /// <summary>
    /// Fetches the list with all jobs of type <typeparamref name="TJob"/>
    /// </summary>
    /// <returns>List of jobs</returns>
    Task<IReadOnlyCollection<TJob>> Execute<TJob, TInput, TOutput>() where TJob : IJob<TInput, TOutput>, new();
}

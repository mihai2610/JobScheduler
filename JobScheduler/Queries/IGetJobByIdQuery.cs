using JobScheduler.Models;

namespace JobScheduler.Queries;

/// <summary>
/// Fetches a job
/// </summary>
public interface IGetJobByIdQuery
{
    /// <summary>
    /// Method to fetch a job of type <typeparamref name="TJob"/> based on <paramref name="jobId"/> 
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    Task<TJob> Execute<TJob, TInput, TOutput>(long jobId) where TJob : IJob<TInput, TOutput>, new();
}

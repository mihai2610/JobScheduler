using JobScheduler.Models;

namespace JobScheduler.Commands;

/// <summary>
/// Command to update the job information
/// </summary>
public interface IUpdateJobCommand
{
    /// <summary>
    /// Updates job infomation
    /// </summary>
    /// <param name="job">Job description</param>
    /// <returns>Updated object</returns>
    Task<TJob> Execute<TJob, TInput, TOutput>(TJob job) where TJob : IJob<TInput, TOutput>, new();
}

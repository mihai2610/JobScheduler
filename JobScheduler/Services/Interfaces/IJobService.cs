using JobScheduler.Models;

namespace JobScheduler.Services.Interfaces
{
    /// <summary>
    /// Service for job operations
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Fetches a job by <paramref name="jobId"/>
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Job info</returns>
        Task<TJob> GetJobById<TJob, TInput, TOutput>(long jobId) where TJob : IJob<TInput, TOutput>, new();

        /// <summary>
        /// Fetches the list of all jobs
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<TJob>> GetAllJobs<TJob, TInput, TOutput>() where TJob : IJob<TInput, TOutput>, new();

        /// <summary>
        /// Creates a new entry in database adding the job information
        /// </summary>
        /// <param name="input">Job input</param>
        /// <returns>New job id</returns>
        Task<TJob> CreateJob<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new();

        /// <summary>
        /// Method to update information about the job
        /// </summary>
        /// <param name="job"></param>
        /// <returns>Updated job</returns>
        Task<TJob> UpdateJob<TJob, TInput, TOutput>(TJob job) where TJob : IJob<TInput, TOutput>, new();
    }
}

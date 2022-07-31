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
        Task<Job> GetJobById(long jobId);

        /// <summary>
        /// Fetches the list of all jobs
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<Job>> GetAllJobs();

        /// <summary>
        /// Creates a new entry in database adding the job information
        /// </summary>
        /// <param name="job"></param>
        /// <returns>New job id</returns>
        Task<long> CreateJob(IReadOnlyCollection<int> input);

        /// <summary>
        /// Method to update information about the job
        /// </summary>
        /// <param name="job"></param>
        /// <returns>Updated job</returns>
        Task<Job> UpdateJob(Job job);
    }
}

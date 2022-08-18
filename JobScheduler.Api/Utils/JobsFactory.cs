using JobScheduler.Api.Extentions;
using JobScheduler.Api.Models;
using JobScheduler.Api.Models.Converters;
using JobScheduler.Extentions;
using JobScheduler.Models;
using JobScheduler.Services.Interfaces;
using LanguageExt.Common;

namespace JobScheduler.Api.Utils;

/// <summary>
/// Factory to generate jobs based on job type
/// </summary>
public interface IJobsFactory
{
    /// <summary>
    /// Return the type of the jon also th type of the input and output based on <paramref name="jobType"/>
    /// </summary>
    /// <param name="jobType">Type of the job that is going to be executed</param>
    /// <param name="input">Input for the job that is going to be executed</param>
    /// <returns>Job type, input type and outptu type</returns>
    Task<Result<JobView>> CreateJob(JobType jobType, object input);
}

/// <inheritdoc/>
public class JobsFactory : IJobsFactory
{
    private readonly IJobService _jobService;

    /// <summary>
    /// Creates an instance of <see cref="IJobService"/>
    /// </summary>
    /// <param name="jobService"></param>
    public JobsFactory(IJobService jobService)
    {
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
    }

    /// <inheritdoc/>
    public async Task<Result<JobView>> CreateJob(JobType jobType, object input) =>
        jobType switch
        {
            JobType.SortListOfLong =>
                 (await Task.FromResult(input.ToModelTypeInput<IReadOnlyCollection<long>>())
                    .BindT(async q => await _jobService.CreateJob<SortListJob, IReadOnlyCollection<long>, IReadOnlyCollection<long>>(q))
                 ).Map(q => q.ToView()),
            _ => throw new NotImplementedException()
        };
}
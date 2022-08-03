using JobScheduler.Commands;
using JobScheduler.Models;
using JobScheduler.Queries;
using JobScheduler.Services.Interfaces;
using LanguageExt.Common;

namespace JobScheduler.Services;

/// <inheritdoc/>
public class JobService : IJobService
{
    private readonly IGetJobByIdQuery _getJobByIdQuery;
    private readonly IGetAllJobsQuery _getAllJobsQuery;
    private readonly ICreateJobCommand _createJobCommand;
    private readonly IUpdateJobCommand _updateJobCommand;

    /// <summary>
    /// Creates an instance of <see cref="JobService"/>
    /// </summary>
    public JobService(
        IGetJobByIdQuery getJobByIdQuery,
        IGetAllJobsQuery getAllJobsQuery,
        ICreateJobCommand createJobCommand,
        IUpdateJobCommand updateJobCommand)
    {
        _getJobByIdQuery = getJobByIdQuery ?? throw new ArgumentNullException(nameof(getJobByIdQuery));
        _getAllJobsQuery = getAllJobsQuery ?? throw new ArgumentNullException(nameof(getAllJobsQuery));
        _createJobCommand = createJobCommand ?? throw new ArgumentNullException(nameof(createJobCommand));
        _updateJobCommand = updateJobCommand ?? throw new ArgumentNullException(nameof(updateJobCommand));
    }

    /// <inheritdoc/>
    public Task<Result<TJob>> CreateJob<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new()
        => _createJobCommand.Execute<TJob, TInput, TOutput>(input);

    /// <inheritdoc/>
    public Task<Result<IReadOnlyCollection<TJob>>> GetAllJobs<TJob, TInput, TOutput>() where TJob : IJob<TInput, TOutput>, new()
        => _getAllJobsQuery.Execute<TJob, TInput, TOutput>();

    /// <inheritdoc/>
    public Task<Result<TJob>> GetJobById<TJob, TInput, TOutput>(long jobId) where TJob : IJob<TInput, TOutput>, new()
        => _getJobByIdQuery.Execute<TJob, TInput, TOutput>(jobId);

    /// <inheritdoc/>
    public Task<Result<TJob>> UpdateJob<TJob, TInput, TOutput>(TJob job) where TJob : IJob<TInput, TOutput>, new()
        => _updateJobCommand.Execute<TJob, TInput, TOutput>(job);
}

﻿using JobScheduler.Commands;
using JobScheduler.Models;
using JobScheduler.Queries;
using JobScheduler.Services.Interfaces;

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
    public Task<long> CreateJob(IReadOnlyCollection<int> input) => _createJobCommand.Execute(input);

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<Job>> GetAllJobs() => _getAllJobsQuery.Execute();

    /// <inheritdoc/>
    public Task<Job> GetJobById(long jobId) => _getJobByIdQuery.Execute(jobId);

    /// <inheritdoc/>
    public Task<Job> UpdateJob(Job job) => _updateJobCommand.Execute(job);
}

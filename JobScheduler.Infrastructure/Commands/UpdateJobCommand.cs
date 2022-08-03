using Dapper;
using JobScheduler.Commands;
using JobScheduler.Exceptions;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Models;
using LanguageExt.Common;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Commands;

/// <inheritdoc/>
public class UpdateJobCommand : IUpdateJobCommand
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates new instance of <see cref="CreateJobCommand"/>
    /// </summary>
    public UpdateJobCommand(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public Task<Result<TJob>> Execute<TJob, TInput, TOutput>(TJob job) where TJob : IJob<TInput, TOutput>, new()
    {
        if(job is null)
        {
            return Task.FromResult(new Result<TJob>(new BadRequestException("Job input cannot be null!")));
        }

        if (job.JobId <= 0)
        {
            return Task.FromResult(new Result<TJob>(new BadRequestException("Job id cannot be less than 1!")));
        }

        if (job.Duration is not null && job.Duration < TimeSpan.Zero)
        {
            return Task.FromResult(new Result<TJob>(new BadRequestException("Duration cannot be negative!")));
        }

        if (job.StartingTime > DateTime.Now)
        {
            return Task.FromResult(new Result<TJob>(new BadRequestException("Job cannot start in the past!")));
        }

        return ExecuteInternal<TJob, TInput, TOutput>(job);
    }

    /// <summary>
    /// For simplicity this method updates only the required values
    /// </summary>
    private async Task<Result<TJob>> ExecuteInternal<TJob, TInput, TOutput>(TJob job) where TJob : IJob<TInput, TOutput>, new()
    {
        using var conn = _context.GetConnection();

        var request = new UpdateJobRequest(
            job.JobId,
            job.Duration?.ToString(),
            job.Status,
            JsonSerializer.Serialize(job.Output));

        var result = await conn.ExecuteAsync(_sql, request);

        return job;
    }

    private record UpdateJobRequest(long JobId, string? Duration, JobStatusType Status, string Output);

    private readonly static string _sql = $@"
            UPDATE Job 
            SET 
                {nameof(UpdateJobRequest.Output)} = @{nameof(UpdateJobRequest.Output)},
                {nameof(UpdateJobRequest.Duration)} = @{nameof(UpdateJobRequest.Duration)},
                {nameof(UpdateJobRequest.Status)} = @{nameof(UpdateJobRequest.Status)}
            WHERE
                JobId = @{nameof(JobDto.JobId)}
        ";
}

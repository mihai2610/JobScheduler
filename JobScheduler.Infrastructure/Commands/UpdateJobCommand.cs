using Dapper;
using JobScheduler.Commands;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Models;
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
    public async Task<Job> Execute(Job job)
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

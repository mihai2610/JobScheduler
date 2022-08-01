using Dapper;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Models;
using JobScheduler.Queries;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Queries;

/// <inheritdoc/>
public class GetAllJobsQuery : IGetAllJobsQuery
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates new instance of <see cref="GetAllJobsQuery"/>
    /// </summary>
    public GetAllJobsQuery(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<Job>> Execute()
    {
        using var conn = _context.GetConnection();

        var result = await conn.QueryAsync<JobDto>(_sql);

        return result.Select(q => new Job(
            JobId: q.JobId,
            StartingTime: DateTime.Parse(q.StartingTime),
            Duration: q.Duration is null ? null : TimeSpan.Parse(q.Duration),
            Status: (JobStatusType)q.Status,
            Input: JsonSerializer.Deserialize<IReadOnlyCollection<int>>(q.Input),
            Output: q.Output is null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<int>>(q.Output)
            )).ToList();
    }

    private readonly static string _sql = $@"
        SELECT
            JobId AS [{nameof(JobDto.JobId)}],
            StartingTime AS [{nameof(JobDto.StartingTime)}],
            Duration AS [{nameof(JobDto.Duration)}],
            Status AS [{nameof(JobDto.Status)}],
            Input AS [{nameof(JobDto.Input)}],
            Output AS [{nameof(JobDto.Output)}]
        FROM 
            Job;
        ";
}

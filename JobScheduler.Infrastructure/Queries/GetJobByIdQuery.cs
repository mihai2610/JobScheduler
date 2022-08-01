using Dapper;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Models;
using JobScheduler.Queries;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Queries;

/// <inheritdoc/>
public class GetJobByIdQuery : IGetJobByIdQuery
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates new instance of <see cref="GetJobByIdQuery"/>
    /// </summary>
    public GetJobByIdQuery(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<Job> Execute(long jobId)
    {
        using var conn = _context.GetConnection();

        var result = await conn.QuerySingleOrDefaultAsync<JobDto>(_sql, new { JobId = jobId });

        return new Job(
            JobId: result.JobId,
            StartingTime: DateTime.Parse(result.StartingTime),
            Duration: result.Duration is null ? null : TimeSpan.Parse(result.Duration),
            Status: (JobStatusType)result.Status,
            Input: JsonSerializer.Deserialize<IReadOnlyCollection<int>>(result.Input),
            Output: result.Output is null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<int>>(result.Output)
        );
    }

    private readonly static string _sql = $@"
        SELECT
            *
        FROM 
            Job
        WHERE 
            JobId = @JobId
        ";
}

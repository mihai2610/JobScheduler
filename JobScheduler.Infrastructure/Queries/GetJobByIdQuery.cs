using Dapper;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Infrastructure.Models.Converters;
using JobScheduler.Models;
using JobScheduler.Queries;

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
    public async Task<TJob> Execute<TJob, TInput, TOutput>(long jobId) where TJob : IJob<TInput, TOutput>, new()
    {
        using var conn = _context.GetConnection();

        var result = await conn.QuerySingleOrDefaultAsync<JobDto>(_sql, new { JobId = jobId });

        return result.ToModel<TJob, TInput, TOutput>();
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

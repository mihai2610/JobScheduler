using Dapper;
using JobScheduler.Exceptions;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Infrastructure.Models.Converters;
using JobScheduler.Models;
using JobScheduler.Queries;
using LanguageExt.Common;

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
    public async Task<Result<IReadOnlyCollection<TJob>>> Execute<TJob, TInput, TOutput>() where TJob : IJob<TInput, TOutput>, new()
    {
        using var conn = _context.GetConnection();

        var result = await conn.QueryAsync<JobDto>(_sql);

        return result.ToModel<TJob, TInput, TOutput>();
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

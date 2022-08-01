using Dapper;
using JobScheduler.Commands;
using JobScheduler.Infrastructure.Models;
using JobScheduler.Infrastructure.Utils;
using JobScheduler.Models;
using System.Text.Json;

namespace JobScheduler.Infrastructure.Commands;

/// <inheritdoc/>
public class CreateJobCommand : ICreateJobCommand
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates new instance of <see cref="CreateJobCommand"/>
    /// </summary>
    public CreateJobCommand(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<long> Execute(IReadOnlyCollection<int> input)
    {
        using var conn = _context.GetConnection();

        var request = new CreateJobReques(
            DateTime.Now,
            JobStatusType.PENDING,
            JsonSerializer.Serialize(input));

        var result = await conn.QuerySingleAsync<long>(_sql, request);

        return result;
    }

    private record CreateJobReques(DateTime StartingTime, JobStatusType Status, string Input);

    private readonly static string _sql = $@"
            INSERT 
                INTO Job ({nameof(CreateJobReques.StartingTime)}, {nameof(CreateJobReques.Status)}, {nameof(CreateJobReques.Input)})
            VALUES
                (@{nameof(CreateJobReques.StartingTime)}, @{nameof(CreateJobReques.Status)}, @{nameof(CreateJobReques.Input)})
            RETURNING JobId;
        ";
}

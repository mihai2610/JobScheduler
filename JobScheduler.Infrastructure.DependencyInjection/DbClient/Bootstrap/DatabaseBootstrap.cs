using Dapper;

namespace JobScheduler.Infrastructure.DependencyInjection.DbClient.Bootstrap;

/// <inheritdoc/>
public class DatabaseBootstrap : IDatabaseBootstrap
{
    private readonly DbContext _context;

    /// <summary>
    /// Creates a new instance of <see cref="DatabaseBootstrap"/>
    /// </summary>
    public DatabaseBootstrap(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public void Setup()
    {
        using var connection = _context.GetConnection();

        var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Job';");
        var tableName = table.FirstOrDefault();
        
        if (!string.IsNullOrEmpty(tableName) && tableName == "Job")
        {
            return;
        }

        connection.Execute(_sql);
    }

    private const string _sql = @"Create Table Job (
                JobId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                StartingTime TEXT NOT NULL,
                Duration TEXT NULL,
                Status INTEGER NOT NULL,
                Input TEXT NOT NULL,
                Output TEXT NULL
            );";
}

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace JobScheduler.Infrastructure.DependencyInjection.DbClient;

public class DbContext
{
    private readonly ConnectionStrings _connection;

    public DbContext(IOptions<ConnectionStrings> connection) =>
        _connection = connection.Value ?? throw new ArgumentNullException(nameof(connection));

    public SqliteConnection GetConnection() => new(_connection.DefaultConnection);
    public string GetConnectionString() => _connection.DefaultConnection.ToString();
}

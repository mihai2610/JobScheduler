namespace JobScheduler.Infrastructure.DependencyInjection.DbClient.Bootstrap;

/// <summary>
/// Provides the inital setup for the database
/// </summary>
public interface IDatabaseBootstrap
{
    /// <summary>
    /// Creates the inial database config before the application starts
    /// </summary>
    public void Setup();
}

using JobScheduler.Models;

namespace JobScheduler.Commands;

/// <summary>
/// Command to create a new job
/// </summary>
public interface ICreateJobCommand
{
    /// <summary>
    /// Method that creates a new job
    /// </summary>
    /// <param name="input"></param>
    /// <returns>New job id</returns>
    Task<long> Execute(IReadOnlyCollection<int> input);
}

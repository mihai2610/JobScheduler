using JobScheduler.Commands;
using JobScheduler.Models;

namespace JobScheduler.Infrastructure.Commands;

/// <inheritdoc/>
public class UpdateJobCommand : IUpdateJobCommand
{
    /// <inheritdoc/>
    public Task<Job> Execute(Job job)
    {
        throw new NotImplementedException();
    }
}

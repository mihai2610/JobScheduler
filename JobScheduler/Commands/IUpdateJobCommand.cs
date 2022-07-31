using JobScheduler.Models;

namespace JobScheduler.Commands;

public interface IUpdateJobCommand
{
    Task<Job> Execute(Job job);
}

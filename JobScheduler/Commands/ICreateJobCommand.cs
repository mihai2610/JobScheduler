using JobScheduler.Models;
using LanguageExt.Common;

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
    Task<Result<TJob>> Execute<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new();
}

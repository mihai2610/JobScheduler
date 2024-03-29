﻿using Dapper;
using JobScheduler.Commands;
using JobScheduler.Exceptions;
using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Models;
using LanguageExt.Common;
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
    public Task<Result<TJob>> Execute<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new()
    {
        if(input is null)
        {
            return Task.FromResult(new Result<TJob>(new BadRequestException("Job input cannot be null!")));
        }

        return ExecuteInternal<TJob, TInput, TOutput>(input);
    }

    private async Task<Result<TJob>> ExecuteInternal<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new()
    {
        using var conn = _context.GetConnection();

        var request = new CreateJobReques(
            DateTime.Now,
            JobStatusType.PENDING,
            JsonSerializer.Serialize(input));

        var newJobId = await conn.QuerySingleAsync<long>(_sql, request);

        return new TJob()
        {
            JobId = newJobId,
            Input = input,
            Duration = null,
            StartingTime = request.StartingTime,
            Status = request.Status
        };
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

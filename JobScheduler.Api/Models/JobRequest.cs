using JobScheduler.Models;
using System.Text.Json.Serialization;

namespace JobScheduler.Api.Models;

/// <summary>
/// Model to describe the job to be performed
/// </summary>
/// <param name="Input">Input of the job to be performed</param>
/// <param name="JobType">Type of the job that is going to be performed</param>
public record JobRequest(
    object Input,
    JobType JobType
);

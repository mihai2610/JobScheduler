using JobScheduler.Api.Extentions;
using JobScheduler.Api.Models;
using JobScheduler.Api.Models.Converters;
using JobScheduler.Api.Utils;
using JobScheduler.Models;
using JobScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Api.Controllers;

/// <summary>
/// Controller to perform job operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/job")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IJobsFactory _jobsFactory;

    /// <summary>
    /// Creates an instance of <see cref="JobController"/>
    /// </summary>
    public JobController(IJobService jobService, IJobsFactory jobsFactory)
    {
        _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        _jobsFactory = jobsFactory ?? throw new ArgumentNullException(nameof(jobsFactory));
    }

    [HttpPost]
    public async Task<ActionResult<JobView>> Create([FromBody] JobRequest job)
    {
        return await _jobsFactory.CreateJob(job.JobType, job.Input);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<JobView>>> GetAll()
    {
        var allJobs = await _jobService.GetAllJobs<ReadOnlyJob, string, string?>();

        return allJobs.ToResponse(q => q.ToView());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<JobView>> GetById([FromRoute] long id)
    {
        var job = await _jobService.GetJobById<ReadOnlyJob, string, string?>(id);

        return job.ToResponse(q => q.ToView());
    }
}

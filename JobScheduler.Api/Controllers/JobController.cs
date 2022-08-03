using JobScheduler.Api.Extentions;
using JobScheduler.Api.Models;
using JobScheduler.Api.Models.Converters;
using JobScheduler.Models;
using JobScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Api.Controllers
{
    /// <summary>
    /// Controller to perform job operations
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/job")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        /// <summary>
        /// Creates an instance of <see cref="JobController"/>
        /// </summary>
        /// <param name="jobService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JobController(IJobService jobService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        }

        [HttpPost]
        public async Task<ActionResult<JobView>> Create([FromBody] JobRequest job)
        {
            var jobId = await _jobService.CreateJob<SortListJob, IReadOnlyCollection<long>, IReadOnlyCollection<long>>(job.Input);

            return jobId.ToResponse(q => q.ToView());
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<JobView>>> GetAll()
        {
            var allJobs = await _jobService.GetAllJobs<SortListJob, IReadOnlyCollection<long>, IReadOnlyCollection<long>>();

            return allJobs.ToResponse(q => q.ToView());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<JobView>> GetById([FromRoute] long id)
        {
            var job = await _jobService.GetJobById<SortListJob, IReadOnlyCollection<long>, IReadOnlyCollection<long>>(id);

            return job.ToResponse(q => q.ToView());
        }
    }
}

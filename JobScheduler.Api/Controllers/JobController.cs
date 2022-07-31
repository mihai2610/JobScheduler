using JobScheduler.Api.Models;
using JobScheduler.Api.Models.Converters;
using JobScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Api.Controllers
{
    /// <summary>
    /// Controller to perform job operations
    /// </summary>
    [ApiController]
    [Route("job")]
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
        public async Task<ActionResult<long>> Create([FromBody]JobRequest job)
        {
            await _jobService.CreateJob(job.Input);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<JobView>>> GetAll()
        {
            var allJobs = await _jobService.GetAllJobs();

            return Ok(allJobs.ToView());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<JobView>> GetById([FromRoute] long id)
        {
            var job = await _jobService.GetJobById(id);

            return Ok(job.ToView());
        }
    }
}

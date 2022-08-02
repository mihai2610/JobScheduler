using JobScheduler.Commands;
using JobScheduler.Infrastructure.DependencyInjection.MqClient;
using JobScheduler.Models;

namespace JobScheduler.Worker
{
    public class Worker<TJob, TInput, TOutput> : BackgroundService where TJob : IJob<TInput, TOutput>, new()
    {
        private readonly ILogger<Worker<TJob, TInput, TOutput>> _logger;
        private readonly IRabbitMqClient _rabbitMqContext;
        private readonly IUpdateJobCommand _updateJobCommand;

        public Worker(
            ILogger<Worker<TJob, TInput, TOutput>> logger,
            IRabbitMqClient rabbitMqContext,
            IUpdateJobCommand updateJobCommand)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rabbitMqContext = rabbitMqContext ?? throw new ArgumentNullException(nameof(rabbitMqContext));
            _updateJobCommand = updateJobCommand ?? throw new ArgumentNullException(nameof(updateJobCommand));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _rabbitMqContext.ConsumeMessage<TJob>(q => ExecuteJob(q));

                _logger.LogInformation("Worker running at: {time} {type}", DateTimeOffset.Now, typeof(TJob));

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ExecuteJob(TJob job)
        {
            var updatedjob = new TJob()
            {
                JobId = job.JobId,
                StartingTime = job.StartingTime,
                Duration = DateTime.Now - job.StartingTime,
                Status = JobStatusType.COMPLETED,
                Output = await job.Execute(job.Input),
                Input = job.Input
            };

            _logger.LogInformation($"Updated job {updatedjob.JobId} {string.Join(",", updatedjob.Output)} {updatedjob.Status}");

            await _updateJobCommand.Execute<TJob, TInput, TOutput>(updatedjob);
        }
    }
}
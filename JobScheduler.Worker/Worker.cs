using JobScheduler.Commands;
using JobScheduler.Infrastructure.DependencyInjection.MqClient;
using JobScheduler.Models;

namespace JobScheduler.Worker
{
    public class Worker<T> : BackgroundService
    {
        private readonly ILogger<Worker<T>> _logger;
        private readonly IRabbitMqContext _rabbitMqContext;
        private readonly IUpdateJobCommand _updateJobCommand;

        public Worker(
            ILogger<Worker<T>> logger,
            IRabbitMqContext rabbitMqContext,
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
                _rabbitMqContext.ConsumeMessage<Job>(ExecuteJob);

                _logger.LogInformation("Worker running at: {time} {type}", DateTimeOffset.Now, typeof(T));

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ExecuteJob(Job job)
        {
            var updatedjob = job with
            {
                Output = job.Input.OrderBy(x => x).ToList(),
                Duration = DateTime.Now - job.StartingTime,
                Status = JobStatusType.COMPLETED
            };

            await _updateJobCommand.Execute(updatedjob);
        }
    }
}
using JobScheduler.Commands;
using JobScheduler.Infrastructure.DependencyInjection.MqClient;
using JobScheduler.Models;

namespace JobScheduler.Infrastructure.Commands.Decorators
{
    /// <summary>
    /// Decorator to publish a new created job to the queue
    /// </summary>
    public class PublishJobCreationDecorator : ICreateJobCommand
    {
        private readonly IRabbitMqContext _mqContext;
        private readonly ICreateJobCommand _createJobCommand;

        public PublishJobCreationDecorator(IRabbitMqContext mqContext, ICreateJobCommand createJobCommand)
        {
            _mqContext = mqContext ?? throw new ArgumentNullException(nameof(mqContext));
            _createJobCommand = createJobCommand ?? throw new ArgumentNullException(nameof(createJobCommand));
        }

        public async Task<long> Execute(IReadOnlyCollection<int> input)
        {
            var newJobId = await _createJobCommand.Execute(input);

            _mqContext.ProduceMessage(new Job(newJobId, DateTime.Now, null, JobStatusType.PENDING, input, null));

            return newJobId;
        }
    }
}

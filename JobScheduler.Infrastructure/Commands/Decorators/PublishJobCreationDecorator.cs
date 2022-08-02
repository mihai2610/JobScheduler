﻿using JobScheduler.Commands;
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

        /// <summary>
        /// Creates an instance of <see cref="PublishJobCreationDecorator"/>
        /// </summary>
        public PublishJobCreationDecorator(IRabbitMqContext mqContext, ICreateJobCommand createJobCommand)
        {
            _mqContext = mqContext ?? throw new ArgumentNullException(nameof(mqContext));
            _createJobCommand = createJobCommand ?? throw new ArgumentNullException(nameof(createJobCommand));
        }

        /// <inheritdoc/>
        public async Task<TJob> Execute<TJob, TInput, TOutput>(TInput input) where TJob : IJob<TInput, TOutput>, new()
        {
            var newJob = await _createJobCommand.Execute<TJob, TInput, TOutput>(input);

            _mqContext.ProduceMessage(newJob);

            return newJob;
        }
    }
}

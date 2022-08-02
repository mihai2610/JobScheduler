namespace JobScheduler.Infrastructure.DependencyInjection.MqClient
{
    /// <summary>
    /// Rabbit Mq context provider
    /// </summary>
    public interface IRabbitMqClient : IDisposable
    {
        /// <summary>
        /// Method to fetch message from the queue and performe some operation over it
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="messageHandler"></param>
        Task ConsumeMessage<T>(Func<T, Task> messageHandler);

        /// <summary>
        /// Method to send a message to the queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        Task ProduceMessage<T>(T message);
    }
}
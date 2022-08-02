using JobScheduler.Commands;
using JobScheduler.Infrastructure.Commands;
using JobScheduler.Infrastructure.DependencyInjection;
using JobScheduler.Models;
using JobScheduler.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IUpdateJobCommand, UpdateJobCommand>();

        services.AddDependencyInjection(
            hostContext.Configuration.GetSection("ConnectionStrings").Bind,
            hostContext.Configuration.GetSection("RabbitMqIdentifier").Bind
        );

        services.AddHostedService<Worker<SortListJob, IReadOnlyCollection<long>, IReadOnlyCollection<long>>>();
    })
    .Build();

await host.RunAsync();

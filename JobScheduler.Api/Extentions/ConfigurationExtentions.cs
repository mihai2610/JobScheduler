using JobScheduler.Infrastructure.DependencyInjection;
using JobScheduler.Infrastructure.Extentions;
using JobScheduler.Services;
using JobScheduler.Services.Interfaces;

namespace JobScheduler.Api.Extentions;

public static class ConfigurationExtentions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Services
        services.AddScoped<IJobService, JobService>();

        //Infrastructure
        services.AddInfrastructure();

        //DI
        services.AddDependencyInjection(
            configuration.GetSection("ConnectionStrings").Bind,
            configuration.GetSection("RabbitMqIdentifier").Bind
        );

        return services;
    }
}

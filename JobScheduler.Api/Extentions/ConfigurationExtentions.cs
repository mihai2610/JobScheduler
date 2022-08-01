using JobScheduler.Infrastructure.DependencyInjection;
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
        services.AddInfrastructure(configuration.GetSection("ConnectionStrings").Bind);

        return services;
    }
}

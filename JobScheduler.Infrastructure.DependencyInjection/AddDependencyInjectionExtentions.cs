using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.DependencyInjection.DbClient.Bootstrap;
using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Infrastructure.DependencyInjection
{
    public static class AddDependencyInjectionExtentions
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services,
            Action<ConnectionStrings> connectionStringsConfig)
        {
            //DBContext
            services.Configure(connectionStringsConfig);
            services.AddSingleton<DbContext>();
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            return services;
        }
    }
}
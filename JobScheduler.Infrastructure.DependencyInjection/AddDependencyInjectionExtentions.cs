using JobScheduler.Infrastructure.DependencyInjection.DbClient;
using JobScheduler.Infrastructure.DependencyInjection.DbClient.Bootstrap;
using JobScheduler.Infrastructure.DependencyInjection.MqClient;
using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Infrastructure.DependencyInjection
{
    public static class AddDependencyInjectionExtentions
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services,
            Action<ConnectionStrings> connectionStringsConfig,
            Action<RabbitMqIdentifier> mqConfig)
        {
            //DBContext
            services.Configure(connectionStringsConfig);
            services.AddSingleton<DbContext>();
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            //MQContext
            services.Configure(mqConfig);
            services.AddSingleton<IRabbitMqContext, RabbitMqContext>();

            return services;
        }
    }
}
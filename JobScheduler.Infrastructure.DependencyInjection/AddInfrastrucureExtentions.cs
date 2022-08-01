using JobScheduler.Commands;
using JobScheduler.Infrastructure.Commands;
using JobScheduler.Infrastructure.DependencyInjection.DapperClient;
using JobScheduler.Infrastructure.DependencyInjection.DapperClient.Bootstrap;
using JobScheduler.Infrastructure.Queries;
using JobScheduler.Infrastructure.Utils;
using JobScheduler.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Infrastructure.DependencyInjection
{
    public static class AddInfrastrucureExtentions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            Action<ConnectionStrings> connectionStringsConfig)
        {
            //DBContext
            services.Configure(connectionStringsConfig);
            services.AddSingleton<DbContext>();
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            //Queries
            services.AddScoped<IGetAllJobsQuery, GetAllJobsQuery>();
            services.AddScoped<IGetJobByIdQuery, GetJobByIdQuery>();

            //Commands
            services.AddScoped<ICreateJobCommand, CreateJobCommand>();
            services.AddScoped<IUpdateJobCommand, UpdateJobCommand>();

            return services;
        }
    }
}
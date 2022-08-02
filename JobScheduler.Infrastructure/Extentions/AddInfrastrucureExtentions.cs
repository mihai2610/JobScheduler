using JobScheduler.Commands;
using JobScheduler.Infrastructure.Commands;
using JobScheduler.Infrastructure.Commands.Decorators;
using JobScheduler.Infrastructure.Queries;
using JobScheduler.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Infrastructure.Extentions
{
    public static class AddInfrastrucureExtentions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //Queries
            services.AddScoped<IGetAllJobsQuery, GetAllJobsQuery>();
            services.AddScoped<IGetJobByIdQuery, GetJobByIdQuery>();

            //Commands
            services.AddScoped<ICreateJobCommand, CreateJobCommand>()
                .Decorate<ICreateJobCommand, PublishJobCreationDecorator>();
            services.AddScoped<IUpdateJobCommand, UpdateJobCommand>();

            return services;
        }
    }
}

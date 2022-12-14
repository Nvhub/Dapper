using DapperProject.Api.Utils;
using DapperProject.Application.Interfaces;
using DapperProject.Infrastructure.Repositories;
using DapperProject.Infrastructure;
using DapperProject.GraphQL;
namespace DapperProject.Api.Services
{
    public static class Dependency
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQRepository, RabbitMQRepository>();
            services.AddSingleton<MemoryCacheUtils>();
            services.AddInfrastructureDependency();
            services.AddGraphQLDependency();
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using DapperProject.Infrastructure.Repositories;
using DapperProject.Application.Interfaces;

namespace DapperProject.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserAuthRepository, UserAuthRepository>();
            return services;


        }
    }
}

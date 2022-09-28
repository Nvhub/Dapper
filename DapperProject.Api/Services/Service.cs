using DapperProject.GraphQL;

namespace DapperProject.Api.Services
{
    public static class Service
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddRazorPages();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            // Local Service
            services.AddJwtService(config);
            services.AddGraphQLService();
            return services;
        }
    }
}

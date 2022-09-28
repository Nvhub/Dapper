using DapperProject.GraphQL.Queries;
using DapperProject.GraphQL.Mutations;
using DapperProject.GraphQL.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DapperProject.GraphQL
{
    public static class Dependency
    {
        public static IServiceCollection AddGraphQLService (this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<UserQuery>()
                .AddMutationType<UserMutation>()
                .AddErrorFilter<ErrorHandler>();
            return services;
        }

        public static IServiceCollection AddGraphQLDependency (this IServiceCollection services)
        {
            return services;
        }
    }
}

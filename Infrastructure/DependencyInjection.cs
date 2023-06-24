using FireplaceApi.Infrastructure.CacheServices;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Gateways;
using FireplaceApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string dbConnectionString)
    {
        services.AddDatabase(dbConnectionString);
        services.AddRepositories();
        services.AddCacheServices();
        services.AddGateways();
    }
}

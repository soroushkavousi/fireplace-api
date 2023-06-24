using FireplaceApi.Application.RequestTraces;
using FireplaceApi.Infrastructure.CacheService;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.CacheServices;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IRequestTraceCacheService, RequestTraceCacheService>();
    }
}

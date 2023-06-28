using FireplaceApi.Domain.Errors;
using FireplaceApi.Infrastructure.CacheService;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace FireplaceApi.Infrastructure.CacheServices;

public static class DependencyInjection
{
    public static void AddCacheServices(this IServiceCollection services)
    {
        var repositoryTypes = Tools.Utils.FindChildTypes<ICacheService>();
        foreach (var repositoryType in repositoryTypes)
        {
            var repositoryInterfaceType = repositoryType.GetInterfaces()
                .Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICacheService<>));
            var repositoryServiceType = repositoryInterfaceType.GetGenericArguments().Single();
            if (!repositoryType.IsAssignableTo(repositoryServiceType))
                throw new InternalServerException($"Interface is not correct. {repositoryType.Name} : {repositoryServiceType.Name}");
            services.AddSingleton(repositoryServiceType, repositoryType);
        }
    }
}

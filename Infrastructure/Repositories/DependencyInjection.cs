using FireplaceApi.Domain.Errors;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace FireplaceApi.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        var repositoryTypes = Tools.Utils.FindChildTypes<IRepository>();
        foreach (var repositoryType in repositoryTypes)
        {
            var repositoryInterfaceType = repositoryType.GetInterfaces()
                .Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRepository<>));
            var repositoryServiceType = repositoryInterfaceType.GetGenericArguments().Single();
            if (!repositoryType.IsAssignableTo(repositoryServiceType))
                throw new InternalServerException($"Interface is not correct. {repositoryType.Name} : {repositoryServiceType.Name}");
            services.AddScoped(repositoryServiceType, repositoryType);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.Entities;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, string dbConnectionString)
    {
        var infrastructureAssemblyName = $"{nameof(FireplaceApi)}.{nameof(Infrastructure)}";
        services.AddDbContext<ApiDbContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(dbConnectionString,
                optionsBuilder => optionsBuilder.MigrationsAssembly(infrastructureAssemblyName))
        );
    }
}

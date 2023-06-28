using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.Tools;

public static class DependencyInjection
{
    public static void AddTools(this IServiceCollection services)
    {
        services.AddScoped<IIdGenerator, ApiIdGenerator>();
    }
}

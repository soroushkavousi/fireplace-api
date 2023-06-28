using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.Loggers;

public static class DependencyInjection
{
    public static void AddLoggers(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IServerLogger<>), typeof(ServerLogger<>));
    }
}

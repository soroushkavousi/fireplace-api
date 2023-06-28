using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.HostedServices;

public static class DependencyInjection
{
    public static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<ReadinessCheckerHostedService>();
        services.AddHostedService<ServerInstanceHandlerHostedService>();
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.HostedServices;

public static class DependencyInjection
{
    public static void AddHostedServices(this IServiceCollection services)
    {
        // Add a hosted service to check the API readiness
        services.AddHostedService<ReadinessCheckerService>();
    }
}

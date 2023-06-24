using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.Auth;

public static class SessionDependencyInjection
{
    public static void AddApiSession(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, SessionAuthorizationHandler>();
    }
}

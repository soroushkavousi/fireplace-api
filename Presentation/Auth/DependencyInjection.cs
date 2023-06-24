using FireplaceApi.Domain.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FireplaceApi.Presentation.Auth;

public static class DependencyInjection
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddApiAuthentication();
        services.AddApiAuthorization();
    }

    private static void AddApiAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddApiCookieAuthentication();
    }

    private static void AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options => options.AddApiPolicies());
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiAuthorizationMiddlewareResultHandler>();

        // Prevent non active sessions
        services.AddApiSession();

        // For XSRF/CSRF attacks, add access to IAntiforgery
        services.AddApiAntiforgery();
    }

    private static void AddApiPolicies(this AuthorizationOptions options)
    {
        options.InvokeHandlersAfterFailure = false;
        options.AddPolicy(AuthConstants.UnverifiedUserPolicyKey, pb =>
        {
            pb.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.NameIdentifier)
                .RequireClaim(AuthConstants.SessionIdClaimKey)
                .RequireClaim(AuthConstants.UserStateClaimKey)
                .AddRequirements(AntiforgeryAuthorizationRequirement.Instance)
                .AddRequirements(SessionAuthorizationRequirement.Instance);
        });
        options.AddPolicy(AuthConstants.UserPolicyKey, pb =>
        {
            pb.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.NameIdentifier)
                .RequireClaim(AuthConstants.SessionIdClaimKey)
                .RequireClaim(AuthConstants.UserStateClaimKey, UserState.VERIFIED.ToString())
                .AddRequirements(AntiforgeryAuthorizationRequirement.Instance)
                .AddRequirements(SessionAuthorizationRequirement.Instance);
        });
    }
}

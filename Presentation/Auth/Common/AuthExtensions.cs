using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace FireplaceApi.Presentation.Auth;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiAuthorizationMiddlewareResultHandler>();
        services.AddScoped<IAuthorizationHandler, SessionAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, AntiforgeryAuthorizationHandler>();
        return services;
    }

    public static void AddApiPolicies(this AuthorizationOptions options)
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

    public static RequestingUser GetRequestingUser(this HttpContext httpContext)
    {
        var hasUser = httpContext.Items.TryGetValue(AuthConstants.RequestingUserKey, out var userObject);
        if (hasUser)
            return userObject.To<RequestingUser>();

        var requestingUser = httpContext.User?.ToRequestingUser();
        httpContext.Items[AuthConstants.RequestingUserKey] = requestingUser;
        return requestingUser;
    }
}

using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Extensions;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class FirewallFieldMiddleware
{
    private readonly FieldDelegate _next;
    private readonly ILogger<FirewallFieldMiddleware> _logger;

    public FirewallFieldMiddleware(FieldDelegate next,
        ILogger<FirewallFieldMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(IMiddlewareContext context)
    {
        if (!context.IsApiResolver())
        {
            await _next(context);
            return;
        }
        var sw = Stopwatch.StartNew();
        AuthenticateRequestingUser(context);
        await _next(context);
        _logger.LogAppInformation(sw: sw, title: "GRAPHQL_FIREWALL_MIDDLEWARE");
    }

    private void AuthenticateRequestingUser(IMiddlewareContext context)
    {
        var isUserEndpoint = context.IsResolverAUserEndpoint();
        if (!isUserEndpoint)
            return;
        var requestingUser = context.ContextData["User"].To<User>();
        var httpContext = (HttpContext)context.ContextData["HttpContext"];
        //var accessTokenValue = httpContext.GetAccessTokenValue();

        //To Do
        //if (isUserEndpoint)
        //    _firewall.ValidateRequestingUserExists(requestingUser, accessTokenValue);
    }
}

public static class FirewallFieldMiddlewareExtensions
{
    public static bool IsResolverAUserEndpoint(this IMiddlewareContext context)
    {
        var field = context.GetField();
        var isUserEndpoint = field.ResolverMember.GetCustomAttribute<AllowAnonymousAttribute>() == null;
        return isUserEndpoint;
    }
}

using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares
{
    public class FirewallFieldMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly ILogger<FirewallFieldMiddleware> _logger;
        private readonly Firewall _firewall;

        public FirewallFieldMiddleware(FieldDelegate next,
            ILogger<FirewallFieldMiddleware> logger, Firewall firewall)
        {
            _next = next;
            _logger = logger;
            _firewall = firewall;
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
            var accessTokenValue = httpContext.GetAccessTokenValue();
            if (isUserEndpoint)
                _firewall.ValidateRequestingUserExists(requestingUser, accessTokenValue);
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
}

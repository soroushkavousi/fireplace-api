using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    public class FirewallMiddleware
    {
        private readonly ILogger<FirewallMiddleware> _logger;
        private readonly RequestDelegate _next;

        public FirewallMiddleware(ILogger<FirewallMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, Firewall firewall)
        {
            var sw = Stopwatch.StartNew();
            var httpMethod = new HttpMethod(httpContext.Request.Method);
            if (httpMethod == HttpMethod.Post
                || httpMethod == HttpMethod.Put
                || httpMethod == HttpMethod.Patch)
            {
                string requestBody = null;
                if (httpContext.Request.ContentType == "application/json"
                    || httpContext.Request.ContentType == "application/merge-patch+json")
                {
                    requestBody = await httpContext.Request.ReadRequestBodyAsync();
                    firewall.CheckRequestJsonBody(requestBody);
                }

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    firewall.CheckRequestContentType(httpContext.Request);
                }
            }

            var inputHeaderParameters = httpContext.GetInputHeaderParameters();
            var inputCookieParameters = httpContext.GetInputCookieParameters();
            var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var ipAddress = inputHeaderParameters.IpAddress;

            if (IsUserEndpoint(httpContext))
            {
                var requesterUser = await firewall.CheckUser(accessTokenValue, ipAddress);
                httpContext.Items[Tools.Constants.RequesterUserKey] = requesterUser;
            }
            else
            {
                await firewall.CheckGuest(ipAddress);
            }

            _logger.LogInformation(sw, "Execution time for inner of the firewall only");
            await _next(httpContext);
        }

        public bool IsUserEndpoint(HttpContext httpContext)
        {
            var endpoint = httpContext.Request.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null)
                return true;
            return false;
        }

        protected string FindAccessTokenValue(ControllerInputHeaderParameters inputHeaderParameters,
            ControllerInputCookieParameters inputCookieParameters)
        {
            var accessTokenValue = inputHeaderParameters.AccessTokenValue;
            if (string.IsNullOrWhiteSpace(accessTokenValue))
                accessTokenValue = inputCookieParameters.AccessTokenValue;
            return accessTokenValue;
        }
    }

    public static class IApplicationBuilderFirewallMiddleware
    {
        public static IApplicationBuilder UseFirewallMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirewallMiddleware>();
        }
    }
}

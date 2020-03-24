using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using GamingCommunityApi.Api.Controllers.Parameters;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Interfaces;
using GamingCommunityApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GamingCommunityApi.Core.Tools;

namespace GamingCommunityApi.Api.Middlewares
{
    public class FirewallMiddleware
    {
        private readonly RequestDelegate _next;

        public FirewallMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, Firewall firewall)
        {
            if (httpContext.Request.ContentType == "application/json")
            {
                var requestBody = await httpContext.Request.ReadRequestBodyAsync();
                firewall.CheckRequestJsonBody(requestBody);
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

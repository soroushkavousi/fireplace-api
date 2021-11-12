using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
            await ControlRequestBody(httpContext, firewall);

            var inputHeaderParameters = httpContext.GetInputHeaderParameters();
            var inputCookieParameters = httpContext.GetInputCookieParameters();
            var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var ipAddress = inputHeaderParameters.IpAddress;

            User requestingUser = null;
            AccessToken accessToken = null;
            var isUserEndpoint = IsUserEndpoint(httpContext);
            if (!string.IsNullOrWhiteSpace(accessTokenValue))
            {
                accessToken = await firewall.ValidateAccessTokenAsync(accessTokenValue, isUserEndpoint);
                if (accessToken != null)
                {
                    requestingUser = accessToken.User;
                    requestingUser.AccessTokens = new List<AccessToken> { accessToken };
                    httpContext.Items[Tools.Constants.RequestingUserKey] = requestingUser;
                }
            }

            if (isUserEndpoint)
            {
                firewall.ValidateRequestingUserExists(requestingUser, accessTokenValue);
                await firewall.CheckUser(requestingUser, ipAddress);
            }
            else
            {
                await firewall.CheckGuest(ipAddress);
            }

            _logger.LogInformation(sw, "Execution time for inner of the firewall only");
            await _next(httpContext);
        }

        private async Task ControlRequestBody(HttpContext httpContext, Firewall firewall)
        {
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
                    CheckRequestJsonBody(requestBody);
                }

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    CheckRequestContentType(httpContext.Request);
                }
            }
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

        public void CheckRequestContentType(HttpRequest request)
        {
            if (request.ContentType == null
                || (
                    request.ContentType.Contains("application/json") == false
                    && request.ContentType.Contains("multipart/form-data") == false
                    && request.ContentType.Contains("application/merge-patch+json") == false
                    ))
            {
                var serverMessage = $"Input request content type is not valid! request.ContentType: {request.ContentType}";
                throw new ApiException(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, serverMessage);
            }
        }

        public void CheckRequestJsonBody(string requestJsonBody)
        {
            if (requestJsonBody.IsJson() == false)
            {
                var serverMessage = $"Input request body is not json! requestJsonBody: {requestJsonBody}";
                throw new ApiException(ErrorName.REQUEST_BODY_IS_NOT_JSON, serverMessage);
            }
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

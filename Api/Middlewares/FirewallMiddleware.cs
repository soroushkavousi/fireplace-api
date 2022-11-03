using FireplaceApi.Api.Attributes;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public async Task InvokeAsync(HttpContext httpContext, Firewall firewall, IAntiforgery antiforgery)
        {
            var sw = Stopwatch.StartNew();
            var isUserEndpoint = httpContext.GetActionAttribute<IAllowAnonymous>() == null;
            ValidateCsrfToken(httpContext, isUserEndpoint);
            await ControlRequestBody(httpContext, firewall);

            var inputHeaderParameters = httpContext.GetInputHeaderParameters();
            var inputCookieParameters = httpContext.GetInputCookieParameters();
            var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var ipAddress = inputHeaderParameters.IpAddress;

            User requestingUser = null;
            AccessToken accessToken = null;
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

            if (isUserEndpoint || requestingUser != null)
            {
                firewall.ValidateRequestingUserExists(requestingUser, accessTokenValue);
                await firewall.CheckUser(requestingUser, ipAddress);
            }
            else
            {
                await firewall.CheckGuest(ipAddress);
            }

            GenerateAndSetCsrfTokenAsCookie(httpContext, antiforgery);
            _logger.LogAppInformation("Execution time for inner of the firewall only", sw);
            await _next(httpContext);
        }

        private async Task ControlRequestBody(HttpContext httpContext, Firewall firewall)
        {
            var httpMethod = new HttpMethod(httpContext.Request.Method);
            if (httpMethod == HttpMethod.Post
                || httpMethod == HttpMethod.Put
                || httpMethod == HttpMethod.Patch)
            {
                CheckRequestContentType(httpContext.Request);
                if (httpContext.Request.ContentType.Contains("application/json")
                    || httpContext.Request.ContentType.Contains("application/merge-patch+json"))
                {
                    var requestBody = await httpContext.Request.ReadRequestBodyAsync();
                    ValidateRequestBodyIsJson(requestBody);
                }
            }
        }

        protected string FindAccessTokenValue(InputHeaderParameters inputHeaderParameters,
            InputCookieParameters inputCookieParameters)
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

        public void ValidateRequestBodyIsJson(string requestJsonBody)
        {
            if (requestJsonBody.IsJson() == false)
            {
                var serverMessage = $"Input request body is not json! requestJsonBody: {requestJsonBody}";
                throw new ApiException(ErrorName.REQUEST_BODY_IS_NOT_JSON, serverMessage);
            }
        }

        private void ValidateCsrfToken(HttpContext httpContext, bool isUserEndpoint)
        {
            if (!isUserEndpoint)
                return;

            if (httpContext.Request.Method.IsSafeHttpMethod())
                return;

            httpContext.Request.Headers.TryGetValue(Tools.Constants.CsrfTokenKey, out StringValues headerCsrfTokenStringValues);
            var headerCsrfToken = headerCsrfTokenStringValues.ToString();
            httpContext.Request.Cookies.TryGetValue(Tools.Constants.CsrfTokenKey, out string cookieCsrfToken);
            if (headerCsrfToken != cookieCsrfToken)
            {
                var serverMessage = $"headerCsrfToken != cookieCsrfToken => {headerCsrfToken} != {cookieCsrfToken}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        private void GenerateAndSetCsrfTokenAsCookie(HttpContext httpContext, IAntiforgery antiforgery)
        {
            if (httpContext.GetActionAttribute<ProducesCsrfTokenAttribute>() == null
                && httpContext.Request.Method.IsSafeHttpMethod())
                return;

            var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
            if (DoesCsrfTokenExistInResponseCookies(httpContext.Response))
                return;
            var newCsrfToken = tokenSet.RequestToken!;
            var cookieOptions = new CookieOptions
            {
                MaxAge = new System.TimeSpan(
                    Configs.Current.Api.CookieMaxAgeInDays, 0, 0, 0),
                HttpOnly = false,
            };
            httpContext.Response.Cookies.Append(Tools.Constants.CsrfTokenKey, newCsrfToken,
                cookieOptions);
        }

        private static bool DoesCsrfTokenExistInResponseCookies(HttpResponse httpResponse)
        {
            if (httpResponse.Headers.TryGetValue(Tools.Constants.SetCookieHeaderKey, out StringValues cookies))
            {
                return cookies.Any(c => c.StartsWith(Tools.Constants.CsrfTokenKey));
            }
            return false;
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

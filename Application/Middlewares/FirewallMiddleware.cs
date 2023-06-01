using FireplaceApi.Application.Attributes;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
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

namespace FireplaceApi.Application.Middlewares;

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
        var firewallCheckStopWatch = Stopwatch.StartNew();
        var isUserEndpoint = httpContext.GetActionAttribute<IAllowAnonymous>() == null;
        var requestPath = httpContext.Request.Path.Value;
        if (!requestPath.StartsWith(Tools.Constants.GraphQLBaseRoute))
            ValidateCsrfToken(httpContext, isUserEndpoint);
        await ControlRequestBody(httpContext, firewall);

        var accessTokenValue = httpContext.GetAccessTokenValue();
        var inputHeaderParameters = httpContext.GetInputHeaderParameters();
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

        if (isUserEndpoint)
        {
            if (!requestPath.StartsWith(Tools.Constants.GraphQLBaseRoute))
                firewall.ValidateRequestingUserExists(requestingUser, accessTokenValue);

            if (requestingUser != null)
                await firewall.CheckUser(requestingUser, ipAddress);
        }
        else
        {
            await firewall.CheckGuest(ipAddress);
        }

        GenerateAndSetCsrfTokenAsCookie(httpContext, antiforgery);
        _logger.LogAppInformation(sw: firewallCheckStopWatch, title: "FIREWALL_CHECK_DURATION");
        await _next(httpContext);
        _logger.LogAppInformation(sw: sw, title: "FIREWALL_MIDDLEWARE");
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

    public void CheckRequestContentType(HttpRequest request)
    {
        if (request.ContentType == null
            || (
                request.ContentType.Contains("application/json") == false
                && request.ContentType.Contains("multipart/form-data") == false
                && request.ContentType.Contains("application/merge-patch+json") == false
                ))
            throw new RequestContentTypeIncorrectValueException(request.ContentType);
    }

    public void ValidateRequestBodyIsJson(string requestJsonBody)
    {
        if (requestJsonBody.IsJson() == false)
            throw new RequestBodyInvalidFormatException();
    }

    private void ValidateCsrfToken(HttpContext httpContext, bool isUserEndpoint)
    {
        if (!isUserEndpoint)
            return;

        if (httpContext.Request.Method.IsSafeHttpMethod())
            return;

        httpContext.Request.Cookies.TryGetValue(Tools.Constants.CsrfTokenKey, out string cookieCsrfToken);
        if (string.IsNullOrWhiteSpace(cookieCsrfToken))
            return;

        httpContext.Request.Headers.TryGetValue(Tools.Constants.CsrfTokenKey, out StringValues headerCsrfTokenStringValues);
        var headerCsrfToken = headerCsrfTokenStringValues.ToString();
        if (headerCsrfToken != cookieCsrfToken)
            throw new CsrfTokenAuthenticationFailedException(headerCsrfToken, cookieCsrfToken);
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

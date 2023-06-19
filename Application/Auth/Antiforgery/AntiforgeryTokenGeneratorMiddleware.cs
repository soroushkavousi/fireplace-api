using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Auth;

public class AntiforgeryTokenGeneratorMiddleware
{
    private readonly ILogger<AntiforgeryTokenGeneratorMiddleware> _logger;
    private readonly RequestDelegate _next;

    public AntiforgeryTokenGeneratorMiddleware(ILogger<AntiforgeryTokenGeneratorMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IAntiforgery antiforgery)
    {
        var sw = Stopwatch.StartNew();
        //Inject CSRF token Before response has been started
        GenerateAndSetCsrfTokenAsCookie(httpContext, antiforgery);
        await _next(httpContext);
        _logger.LogAppInformation(sw: sw, title: "ANTIFORGERY_MIDDLEWARE");
    }

    private static void GenerateAndSetCsrfTokenAsCookie(HttpContext httpContext, IAntiforgery antiforgery)
    {
        if (httpContext.Request.Method.IsSafeHttpMethod()
            && httpContext.GetActionAttribute<ProducesCsrfTokenAttribute>() == null)
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
        httpContext.Response.Cookies.Append(AntiforgeryConstants.CsrfTokenKey, newCsrfToken,
            cookieOptions);
    }

    private static bool DoesCsrfTokenExistInResponseCookies(HttpResponse httpResponse)
    {
        if (httpResponse.Headers.TryGetValue(Tools.Constants.ResponseSetCookieHeaderKey, out StringValues cookies))
        {
            return cookies.Any(c => c.StartsWith(AntiforgeryConstants.CsrfTokenKey));
        }
        return false;
    }
}

public static class IApplicationBuilderAntiforgeryTokenGeneratorMiddleware
{
    public static IApplicationBuilder UseAntiforgeryTokenGenerator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AntiforgeryTokenGeneratorMiddleware>();
    }
}

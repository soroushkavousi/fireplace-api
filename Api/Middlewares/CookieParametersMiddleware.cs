using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Operators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    public class CookieParametersMiddleware
    {
        private readonly ILogger<CookieParametersMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CookieParametersMiddleware(ILogger<CookieParametersMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var sw = Stopwatch.StartNew();
            GetInputCookies(httpContext);
            await _next(httpContext);
            SetOutputCookies(httpContext);
            _logger.LogTrace(sw);
        }

        private static void GetInputCookies(HttpContext httpContext)
        {
            var inputCookieParameters = new InputCookieParameters(
                httpContext);
            httpContext.Items[Constants.InputCookieParametersKey] =
                inputCookieParameters;
        }

        private void SetOutputCookies(HttpContext httpContext)
        {
            var outputCookieParameters = httpContext.Items
                .GetValue(Constants.OutputCookieParametersKey, null)?
                .To<IOutputCookieParameters>();
            if (outputCookieParameters == null)
                return;
            var cookieCollection = outputCookieParameters.GetCookieCollection();
            if (cookieCollection == null || cookieCollection.Count == 0)
                return;

            var cookieOptions = new CookieOptions
            {
                MaxAge = new System.TimeSpan(
                    GlobalOperator.GlobalValues.Api.CookieMaxAgeInDays, 0, 0, 0)
            };
            foreach (Cookie cookie in cookieCollection)
            {
                httpContext.Response.Cookies.Append(cookie.Name, cookie.Value,
                    cookieOptions);
            }
        }
    }

    public static class IApplicationBuilderCookieParametersMiddleware
    {
        public static IApplicationBuilder UseCookieParametersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieParametersMiddleware>();
        }
    }
}

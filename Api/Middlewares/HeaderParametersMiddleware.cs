using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    public class HeaderParametersMiddleware
    {
        private readonly ILogger<HeaderParametersMiddleware> _logger;
        private readonly RequestDelegate _next;

        public HeaderParametersMiddleware(ILogger<HeaderParametersMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var sw = Stopwatch.StartNew();
            GetInputHeaders(httpContext);
            await _next(httpContext);
            SetOutputHeaders(httpContext);
            _logger.LogTrace(sw);
        }

        private static void GetInputHeaders(HttpContext httpContext)
        {
            var inputHeaderParameters = new ControllerInputHeaderParameters(httpContext);
            httpContext.Items[Constants.ControllerInputHeaderParametersKey] = inputHeaderParameters;
        }

        private void SetOutputHeaders(HttpContext httpContext)
        {
            var outputHeaderParameters = httpContext.Items.GetValue(Constants.ControllerOutputHeaderParametersKey, null)?.To<IControllerOutputHeaderParameters>();
            if (outputHeaderParameters == null)
                return;
            var headerDictionary = outputHeaderParameters.GetHeaderDictionary();
            if (headerDictionary == null || headerDictionary.Count == 0)
                return;
            foreach (var header in headerDictionary)
            {
                httpContext.Response.Headers.Add(header);
            }
        }
    }

    public static class IApplicationBuilderHeaderParametersMiddleware
    {
        public static IApplicationBuilder UseHeaderParametersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderParametersMiddleware>();
        }
    }
}

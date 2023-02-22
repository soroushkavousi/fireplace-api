using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Operators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares
{
    public class RequestTracerMiddleware
    {
        private readonly ILogger<RequestTracerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestTracerMiddleware(ILogger<RequestTracerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RequestTraceOperator requestTraceOperator)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogAppInformation(title: "REQUEST");
            await _next(context);
            await StoreRequestTrace(context, sw, requestTraceOperator);
            _logger.LogAppInformation(sw: sw, title: "REQUEST_DURATION");
        }

        private async Task StoreRequestTrace(HttpContext context, Stopwatch sw,
            RequestTraceOperator requestTraceOperator)
        {
            var method = context.Request.Method;
            var action = FindActionName(context);
            var url = context.Request.GetDisplayUrl();
            var ip = context.GetInputHeaderParameters()?.IpAddress;
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var userId = context.GetRequestingUser()?.Id;
            var statusCode = context.Response.StatusCode;
            var errorName = context.GetError()?.Name;
            var duration = sw.Finish();
            var requestTrace = await requestTraceOperator.CreateRequestTraceAsync(method,
                url, ip, userAgent, userId, statusCode, duration, action, errorName);
        }

        private string FindActionName(HttpContext context)
        {
            var routeData = context.GetRouteData();
            routeData.Values.TryGetValue("action", out var actionName);
            return (string)actionName;
        }
    }

    public static class IApplicationBuilderRequestTracerMiddleware
    {
        public static IApplicationBuilder UseRequestTracerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTracerMiddleware>();
        }
    }
}

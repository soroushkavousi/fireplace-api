using FireplaceApi.Application.Auth;
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

namespace FireplaceApi.Application.Middlewares;

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
        var requestInfoMessage = context.CreateRequestInfoMessage();
        _logger.LogAppInformation(requestInfoMessage, title: "REQUEST");
        await _next(context);
        var action = context.FindActionName();
        await StoreRequestTrace(context, sw, requestTraceOperator);
        _logger.LogAppInformation($"action: {action ?? "null"} | {requestInfoMessage}", sw: sw, title: "REQUEST_DURATION");
    }

    private async Task StoreRequestTrace(HttpContext context, Stopwatch sw,
        RequestTraceOperator requestTraceOperator)
    {
        var method = context.Request.Method;
        var action = context.FindActionName();
        var url = context.Request.GetDisplayUrl();
        var ip = context.GetInputHeaderDto()?.IpAddress;
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var userId = context.GetRequestingUser()?.Id;
        var statusCode = context.Response.StatusCode;
        var errorType = context.GetError()?.Type;
        var errorField = context.GetError()?.Field;
        var duration = sw.Finish();
        var requestTrace = await requestTraceOperator.CreateRequestTraceAsync(method,
            url, ip, userAgent, userId, statusCode, duration, action, errorType, errorField);
    }
}

public static class RequestTracerMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTracer(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestTracerMiddleware>();
    }

    public static string CreateRequestInfoMessage(this HttpContext context)
    {
        var ip = context.GetInputHeaderDto()?.IpAddress;
        var request = context.Request;
        var method = request.Method;
        var url = $"{request.Scheme}://{request.Host}{request.Path}";
        var requestInfoMessage = $"{ip} {method} {url}";
        return requestInfoMessage;
    }

    public static string FindActionName(this HttpContext context)
    {
        string actionName = null;
        if (context.Items.TryGetValue(Tools.Constants.ActionNameKey, out var actionNameObject))
        {
            actionName = (string)actionNameObject;
            return actionName;
        }

        var routeData = context.GetRouteData();
        if (routeData.Values.TryGetValue("action", out actionNameObject))
            actionName = (string)actionNameObject;
        else
        {
            var requestPath = context.Request.Path.ToString();
            if (requestPath.StartsWith(Tools.Constants.GraphQLBaseRoute))
                actionName = "GraphQL";
        }
        context.Items.Add(Tools.Constants.ActionNameKey, actionName);
        return actionName;
    }
}

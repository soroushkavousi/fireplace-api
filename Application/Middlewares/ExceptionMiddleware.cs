using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ErrorOperator errorOperator)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var error = await errorOperator.GetErrorAsync(ex);
            httpContext.Items[Constants.ErrorKey] = error;
            await ReportError(error, httpContext);
        }
        _logger.LogAppTrace(sw: sw, title: "EXCEPTION_MIDDLEWARE");
    }

    private async Task ReportError(Error error, HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = error.HttpStatusCode;

        if (error.Type != ErrorType.INTERNAL_SERVER)
            _logger.LogAppWarning(message: $"{error.Type}: {error.ServerMessage}", title: "CLIENT_ERROR", parameters: new { error.Field, error.Parameters });
        else
        {
            if (error.Exception.GetType().IsSubclassOf(typeof(ApiException)))
                _logger.LogAppError($"{error.Type}: {error.ServerMessage}", title: "SERVER_ERROR", parameters: new { error.Field, error.Parameters }, ex: error.Exception);
            else
                _logger.LogAppCritical($"{error.Type}: {error.ServerMessage}", title: "UNKNOWN_ERROR", parameters: new { error.Field, error.Parameters }, ex: error.Exception);
        }

        var apiExceptionErrorDto = error.ToApiExceptionDto();
        await httpContext.Response.WriteAsync(apiExceptionErrorDto.ToJson(ignoreSensitiveLimit: true));
    }
}

public static class IApplicationBuilderExceptionMiddleware
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}

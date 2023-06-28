using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class ApiExceptionHandlerMiddleware
{
    private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ApiExceptionHandlerMiddleware(ILogger<ApiExceptionHandlerMiddleware> logger, RequestDelegate next)
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
            httpContext.Items[Tools.Constants.ErrorKey] = error;
            await ReportError(error, httpContext);
        }
        _logger.LogServerTrace(sw: sw, title: "EXCEPTION_MIDDLEWARE");
    }

    private async Task ReportError(Error error, HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = error.HttpStatusCode;

        if (error.Type != ErrorType.INTERNAL_SERVER)
            _logger.LogServerWarning(message: $"{error.Type}: {error.ServerMessage}", title: "CLIENT_ERROR", parameters: new { error.Field, error.Parameters });
        else
        {
            if (error.Exception.GetType().IsSubclassOf(typeof(ApiException)))
                _logger.LogServerError($"{error.Type}: {error.ServerMessage}", title: "SERVER_ERROR", parameters: new { error.Field, error.Parameters }, ex: error.Exception);
            else
                _logger.LogServerCritical($"{error.Type}: {error.ServerMessage}", title: "UNKNOWN_ERROR", parameters: new { error.Field, error.Parameters }, ex: error.Exception);
        }

        var apiExceptionErrorDto = error.ToApiExceptionDto();
        await httpContext.Response.WriteAsync(apiExceptionErrorDto.ToJson(ignoreSensitiveLimit: true));
    }
}

public static class IApplicationBuilderApiExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionHandlerMiddleware>();
    }
}

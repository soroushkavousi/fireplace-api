using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ErrorConverter errorConverter,
            ErrorOperator errorOperator)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var apiException = ex switch
                {
                    ApiException apiExceptionObject => apiExceptionObject,
                    _ => new InternalServerException(Error.InternalServerError.ServerMessage, systemException: ex),
                };
                var error = await CreateErrorAsync(apiException, errorOperator);
                httpContext.Items[Constants.ErrorKey] = error;
                await ReportError(error, errorConverter, httpContext);
            }
            _logger.LogAppTrace(sw: sw, title: "EXCEPTION_MIDDLEWARE");
        }

        private async Task<Error> CreateErrorAsync(ApiException apiException, ErrorOperator errorOperator)
        {
            var sw = Stopwatch.StartNew();
            var error = await errorOperator.GetErrorAsync(apiException.ErrorIdentifier);
            if (error == null)
            {
                _logger.LogAppError("Can't fill error details from database!", sw, parameters: apiException);
                var errorTypeGeneralIdentifier = ErrorIdentifier.OfTypeAndField(apiException.ErrorType, FieldName.GENERAL);
                error = await errorOperator.GetErrorAsync(errorTypeGeneralIdentifier);
                error ??= Error.InternalServerError;
            }

            error.Field = apiException.ErrorField;
            error.ServerMessage = apiException.ErrorServerMessage;
            error.Exception = apiException.Exception;
            error.Parameters = apiException.Parameters;
            return error;
        }

        private async Task ReportError(Error error, ErrorConverter errorConverter, HttpContext httpContext)
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

            var apiExceptionErrorDto = errorConverter.ConvertToApiExceptionDto(error);
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
}

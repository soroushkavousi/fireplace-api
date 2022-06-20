using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
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
                    _ => new ApiException(Error.InternalServerError.Name, Error.InternalServerError.ClientMessage, systemException: ex),
                };
                var error = await CreateErrorAsync(apiException, errorOperator);
                await ReportError(error, errorConverter, httpContext);
            }
            _logger.LogAppTrace(sw: sw, title: "EXCEPTION_MIDDLEWARE");
        }

        private async Task<Error> CreateErrorAsync(ApiException apiException, ErrorOperator errorOperator)
        {
            var sw = Stopwatch.StartNew();
            Error error;
            if (await errorOperator.DoesErrorNameExistAsync(apiException.ErrorName))
            {
                error = await errorOperator.GetErrorByNameAsync(apiException.ErrorName);
            }
            else
            {
                error = Error.InternalServerError;
                _logger.LogAppError($"Can't fill error details from database", sw, parameters: apiException); ;
            }
            error.ServerMessage = apiException.ErrorServerMessage;
            error.Exception = apiException.Exception;
            return error;
        }

        private async Task ReportError(Error error, ErrorConverter errorConverter, HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = error.HttpStatusCode;

            if (error.Name != ErrorName.INTERNAL_SERVER)
            {
                _logger.LogAppWarning(message: $"{error.Name}: {error.ServerMessage}", title: "CLIENT_ERROR");
            }
            else
            {
                if (error.Exception.GetType().IsSubclassOf(typeof(Exception)))
                {
                    _logger.LogAppError($"{error.Name}: {error.ServerMessage}", ex: error.Exception, title: "SERVER_ERROR");
                }
                else
                {
                    _logger.LogAppCritical($"{error.Name}: {error.ServerMessage}", ex: error.Exception, title: "EXCEPTION");
                }
            }

            var apiExceptionErrorDto = errorConverter.ConvertToApiExceptionDto(error);
            await httpContext.Response.WriteAsync(apiExceptionErrorDto.ToJson());
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

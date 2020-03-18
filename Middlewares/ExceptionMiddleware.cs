using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Converters;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Operators;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Services;

namespace GamingCommunityApi.Middlewares
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
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var apiException = ex switch
                {
                    ApiException apiExceptionObject => apiExceptionObject,
                    _ => new ApiException(Error.InternalServerError.Name, Error.InternalServerError.ClientMessage, systemException:ex),
                };
                var error = await CreateErrorAsync(apiException, errorOperator);
                await ReportError(error, errorConverter, httpContext);
            }
        }

        private async Task<Error> CreateErrorAsync(ApiException apiException, ErrorOperator errorOperator)
        {
            Error error;
            if (await errorOperator.DoesErrorIdExistAsync(apiException.ErrorId))
            {
                error = await errorOperator.GetErrorByNameAsync(apiException.ErrorId);
            }
            else
            {
                error = Error.InternalServerError;
                _logger.LogError($"Can't fill error details from database | {apiException.ToJson()}");
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
                _logger.LogWarning($"#ClientError | {error.ServerMessage} | {error.ToJson()}");
            }
            else
            {
                if (error.Exception.GetType().IsSubclassOf(typeof(Exception)))
                {
                    _logger.LogError(error.Exception, $"#ServerError | {error.ServerMessage} | {error.ToJson()}");
                }
                else
                {
                    _logger.LogCritical(error.Exception, $"#Exception | {error.ServerMessage} | {error.ToJson()}");
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

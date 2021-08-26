using FireplaceApi.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    /// <summary>
    /// Not used. Just for learning approach.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using var responseBody = new MemoryStream();

            try
            {
                //First, get the incoming request
                var request = await FormatRequest(context.Request);
                _logger.LogInformation(request);
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response);
                _logger.LogInformation(response);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            string requestBodyText;
            if (request.ContentType != null && request.ContentType.Contains("multipart/form-data"))
            {
                requestBodyText = "multipart/form-data body";
            }
            else
            {
                requestBodyText = await request.ReadRequestBodyAsync();
            }

            var requestQueryString = "NoQueryString";
            if (request.QueryString.HasValue)
                requestQueryString = request.QueryString.Value;

            return $"#Request | {requestQueryString} | {requestBodyText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a 
            using var streamReader = new StreamReader(response.Body, encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var responseBodyText = await streamReader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(responseBodyText))
                responseBodyText = "NoBody";
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"#Response | {response.StatusCode} | {responseBodyText}";
        }
    }

    public static class IApplicationBuilderRequestResponseLoggingMiddleware
    {
        public static IApplicationBuilder UseRequestResponseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}

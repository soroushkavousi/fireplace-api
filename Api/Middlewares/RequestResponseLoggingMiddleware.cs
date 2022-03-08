using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    /// <summary>
    /// Not used. Just for learning approach.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger, RequestDelegate next)
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
                await LogRequest(context.Request);

                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                var sw = Stopwatch.StartNew();
                await _next(context);

                await LogResponse(context.Response, sw);
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

        private async Task LogRequest(HttpRequest request)
        {
            var sw = Stopwatch.StartNew();
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

            var requestLogMessage = $"#Request | {requestQueryString} | {requestBodyText}";
            _logger.LogAppInformation(sw, requestLogMessage);
        }

        private async Task LogResponse(HttpResponse response, Stopwatch sw)
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
            var responseLogMessage = $"#Response #ControllerDuration | {response.StatusCode} | {responseBodyText}";
            _logger.LogAppInformation(sw, responseLogMessage);
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

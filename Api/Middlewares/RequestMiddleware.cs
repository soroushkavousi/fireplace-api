using FireplaceApi.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    // For Request Table
    public class RequestMiddleware
    {
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestMiddleware(ILogger<RequestMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await StoreRequestDetails(context.Request);
            await _next(context);
        }

        private async Task StoreRequestDetails(HttpRequest request)
        {
            var sw = Stopwatch.StartNew();
            var requestBodyText = await ExtractRequestBodyTextAsync(request);
            var requestQueryString = ExtractRequestQueryString(request);

            var requestLogMessage = $"{requestQueryString} | {requestBodyText}";
        }

        private static async Task<string> ExtractRequestBodyTextAsync(HttpRequest request)
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
            if (string.IsNullOrWhiteSpace(requestBodyText))
                requestBodyText = "Empty body";

            return requestBodyText;
        }

        private static string ExtractRequestQueryString(HttpRequest request)
        {
            var requestQueryString = "NoQueryString";
            if (request.QueryString.HasValue)
                requestQueryString = request.QueryString.Value;

            return requestQueryString;
        }
    }

    public static class IApplicationBuilderRequestMiddleware
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}

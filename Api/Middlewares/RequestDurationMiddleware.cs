using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    public class RequestDurationMiddleware
    {
        private readonly ILogger<RequestDurationMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestDurationMiddleware(ILogger<RequestDurationMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogAppInformation(title: "REQUEST");
            await _next(context);
            _logger.LogAppInformation(sw: sw, title: "REQUEST_DURATION");
        }
    }

    public static class IApplicationBuilderRequestDurationMiddleware
    {
        public static IApplicationBuilder UseRequestDurationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestDurationMiddleware>();
        }
    }
}

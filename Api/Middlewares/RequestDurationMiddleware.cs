using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    /// <summary>
    /// Not used. Just for learning approach.
    /// </summary>
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
            await _next(context);
            _logger.LogInformation(sw, "#RequestDuration");
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

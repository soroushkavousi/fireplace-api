using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Middlewares
{
    /// <summary>
    /// Not used. Just for learning approach.
    /// </summary>
    public class SessionMiddleware
    {
        private readonly ILogger<SessionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public SessionMiddleware(ILogger<SessionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            await _next(context);
            _logger.LogTrace(sw);
        }
    }

    public static class IApplicationBuilderSessionMiddleware
    {
        public static IApplicationBuilder UseSessionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddleware>();
        }
    }
}

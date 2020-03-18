using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Middlewares
{
    /// <summary>
    /// Not used. Just for learning approach.
    /// </summary>
    public class AddDataToResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public AddDataToResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var existingBody = context.Response.Body;
            try
            {
                var newContent = string.Empty;

                using var newBody = new MemoryStream();
                context.Response.Body = newBody;

                await _next(context);

                context.Response.Body = new MemoryStream();

                newBody.Seek(0, SeekOrigin.Begin);
                context.Response.Body = existingBody;
                using (var streamReader = new StreamReader(newBody))
                {
                    newContent = streamReader.ReadToEnd();
                }

                newContent = $"{{ \"data\": {newContent} }}";

                await context.Response.WriteAsync(newContent);
            }
            catch (Exception)
            {
                context.Response.Body = existingBody;
                throw;
            }
        }
    }

    public static class IApplicationBuilderAddDataToResponseMiddleware
    {
        public static IApplicationBuilder UseAddDataToResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddDataToResponseMiddleware>();
        }
    }
}

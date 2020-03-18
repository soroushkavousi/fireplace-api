using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using GamingCommunityApi.Controllers.Parameters;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Interfaces;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Middlewares
{
    public class HeaderParametersMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderParametersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            GetInputHeaders(httpContext);
            await _next(httpContext);
            SetOutputHeaders(httpContext);
        }

        private static void GetInputHeaders(HttpContext httpContext)
        {
            var inputHeaderParameters = new ControllerInputHeaderParameters(httpContext);
            httpContext.Items[Constants.ControllerInputHeaderParametersKey] = inputHeaderParameters;
        }

        private void SetOutputHeaders(HttpContext httpContext)
        {
            var outputHeaderParameters = httpContext.Items.GetValue(Constants.ControllerOutputHeaderParametersKey, null)?.To<IControllerOutputHeaderParameters>();
            if (outputHeaderParameters == null)
                return;
            var headerDictionary = outputHeaderParameters.GetHeaderDictionary();
            if (headerDictionary == null || headerDictionary.Count == 0)
                return;
            foreach (var header in headerDictionary)
            {
                httpContext.Response.Headers.Add(header);
            }
        }
    }

    public static class IApplicationBuilderHeaderParametersMiddleware
    {
        public static IApplicationBuilder UseHeaderParametersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderParametersMiddleware>();
        }
    }
}

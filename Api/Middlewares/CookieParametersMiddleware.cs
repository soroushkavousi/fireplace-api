using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using FireplaceApi.Api.Controllers.Parameters;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Middlewares
{
    public class CookieParametersMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieParametersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            GetInputCookies(httpContext);
            await _next(httpContext);
            SetOutputCookies(httpContext);
        }

        private static void GetInputCookies(HttpContext httpContext)
        {
            var inputCookieParameters = new ControllerInputCookieParameters(httpContext);
            httpContext.Items[Constants.ControllerInputCookieParametersKey] = inputCookieParameters;
        }

        private void SetOutputCookies(HttpContext httpContext)
        {
            var outputCookieParameters = httpContext.Items.GetValue(Constants.ControllerOutputCookieParametersKey, null)?.To<IControllerOutputCookieParameters>();
            if (outputCookieParameters == null)
                return;
            var cookieCollection = outputCookieParameters.GetCookieCollection();
            if (cookieCollection == null || cookieCollection.Count == 0)
                return;

            foreach (Cookie cookie in cookieCollection)
            {
                httpContext.Response.Cookies.Append(cookie.Name, cookie.Value);
            }
            //var httpContextCookieCollection = httpContext.Response.Cookies.To<ResponseCookies>();
            //httpContextCookieCollection.Add(cookieCollection);
        }
    }

    public static class IApplicationBuilderCookieParametersMiddleware
    {
        public static IApplicationBuilder UseCookieParametersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieParametersMiddleware>();
        }
    }
}

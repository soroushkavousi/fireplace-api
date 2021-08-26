using FireplaceApi.Api.Controllers.Parameters;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models.UserInformations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static ControllerInputHeaderParameters GetInputHeaderParameters(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.ControllerInputHeaderParametersKey].To<ControllerInputHeaderParameters>();
        }

        public static ControllerInputCookieParameters GetInputCookieParameters(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.ControllerInputCookieParametersKey].To<ControllerInputCookieParameters>();
        }

        public static User GetRequesterUser(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.RequesterUserKey].To<User>();
        }

        public static async Task<string> ReadRequestBodyAsync(this HttpRequest request)
        {
            request.EnableBuffering();

            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var requestBodyText = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBodyText;
        }
    }
}

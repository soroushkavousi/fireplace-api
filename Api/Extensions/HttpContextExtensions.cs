using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static InputHeaderParameters GetInputHeaderParameters(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.InputHeaderParametersKey].To<InputHeaderParameters>();
        }

        public static InputCookieParameters GetInputCookieParameters(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.InputCookieParametersKey].To<InputCookieParameters>();
        }

        public static User GetRequestingUser(this HttpContext httpContext)
        {
            return httpContext.Items[Constants.RequestingUserKey].To<User>();
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

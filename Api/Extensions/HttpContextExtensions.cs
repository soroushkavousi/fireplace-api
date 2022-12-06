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
        private static readonly string _inputHeaderParametersKey = "InputHeaderParameters";
        private static readonly string _inputCookieParametersKey = "InputCookieParameters";

        public static InputHeaderParameters GetInputHeaderParameters(this HttpContext httpContext)
        {
            if (httpContext.Items.ContainsKey(_inputHeaderParametersKey))
                return httpContext.Items[_inputHeaderParametersKey].To<InputHeaderParameters>();

            var inputHeaderParameters = new InputHeaderParameters(httpContext);
            httpContext.Items[_inputHeaderParametersKey] = inputHeaderParameters;
            return inputHeaderParameters;
        }

        public static InputCookieParameters GetInputCookieParameters(this HttpContext httpContext)
        {
            if (httpContext.Items.ContainsKey(_inputCookieParametersKey))
                return httpContext.Items[_inputCookieParametersKey].To<InputCookieParameters>();

            var inputCookieParameters = new InputCookieParameters(httpContext);
            httpContext.Items[_inputCookieParametersKey] = inputCookieParameters;
            return inputCookieParameters;
        }

        public static User GetRequestingUser(this HttpContext httpContext)
        {
            User user = null;
            var hasUser = httpContext.Items.TryGetValue(Constants.RequestingUserKey, out var userObject);
            if (hasUser)
                user = userObject.To<User>();
            return user;
        }

        public static Error GetError(this HttpContext httpContext)
        {
            Error error = null;
            var hasError = httpContext.Items.TryGetValue(Constants.ErrorKey, out var errorObject);
            if (hasError)
                error = errorObject.To<Error>();
            return error;
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

        public static T GetActionAttribute<T>(this HttpContext httpContext)
            where T : class
        {
            var endpoint = httpContext.Request.HttpContext.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<T>();
        }
    }
}

using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly string _inputHeaderParametersKey = "InputHeaderParameters";
        private static readonly string _inputCookieParametersKey = "InputCookieParameters";
        private static readonly string _accessTokenKey = "AccessToken";

        public static InputHeaderParameters GetInputHeaderParameters(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue(_inputHeaderParametersKey, out object value))
                return value.To<InputHeaderParameters>();

            var inputHeaderParameters = new InputHeaderParameters(httpContext);
            httpContext.Items[_inputHeaderParametersKey] = inputHeaderParameters;
            return inputHeaderParameters;
        }

        public static InputCookieParameters GetInputCookieParameters(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue(_inputCookieParametersKey, out object value))
                return value.To<InputCookieParameters>();

            var inputCookieParameters = new InputCookieParameters(httpContext);
            httpContext.Items[_inputCookieParametersKey] = inputCookieParameters;
            return inputCookieParameters;
        }

        public static string GetAccessTokenValue(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue(_accessTokenKey, out object value))
                return value.To<string>();

            var inputHeaderParameters = httpContext.GetInputHeaderParameters();
            var inputCookieParameters = httpContext.GetInputCookieParameters();

            var accessTokenValue = inputHeaderParameters.AccessTokenValue;
            if (string.IsNullOrWhiteSpace(accessTokenValue))
                accessTokenValue = inputCookieParameters.AccessTokenValue;

            httpContext.Items[_accessTokenKey] = accessTokenValue;
            return accessTokenValue;
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

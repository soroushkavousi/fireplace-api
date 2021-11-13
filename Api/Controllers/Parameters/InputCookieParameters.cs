using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Controllers
{
    public class InputCookieParameters
    {
        public string AccessTokenValue { get; set; }

        public InputCookieParameters(HttpContext httpContext)
        {
            AccessTokenValue = ExtractAccessTokenValue(httpContext);
        }

        private string ExtractAccessTokenValue(HttpContext httpContext)
        {
            var doesRequestHaveAccessToken = httpContext.Request.Cookies
                .TryGetValue(Constants.ResponseAccessTokenCookieKey, out string accessTokenValue);
            if (!doesRequestHaveAccessToken)
                return null;
            else
                return accessTokenValue;
        }
    }
}

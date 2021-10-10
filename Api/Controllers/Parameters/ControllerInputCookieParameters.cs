using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerInputCookieParameters
    {
        public string AccessTokenValue { get; set; }

        public ControllerInputCookieParameters(HttpContext httpContext)
        {
            AccessTokenValue = ExtractAccessTokenValue(httpContext);
        }

        private string ExtractAccessTokenValue(HttpContext httpContext)
        {
            string accessTokenValue = null;
            return accessTokenValue;
        }
    }
}

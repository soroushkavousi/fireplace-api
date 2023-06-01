using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Attributes;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Application.Controllers;

public class InputCookieParameters
{
    [Sensitive]
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

using FireplaceApi.Domain.Common;
using FireplaceApi.Presentation.Auth;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Presentation.Dtos;

public class InputCookieDTO
{
    [Sensitive]
    public string AccessTokenValue { get; set; }

    public InputCookieDTO(HttpContext httpContext)
    {
        AccessTokenValue = ExtractAccessTokenValue(httpContext);
    }

    private string ExtractAccessTokenValue(HttpContext httpContext)
    {
        var doesRequestHaveAccessToken = httpContext.Request.Cookies
            .TryGetValue(AuthConstants.AccessTokenCookieKey, out string accessTokenValue);
        if (!doesRequestHaveAccessToken)
            return null;
        else
            return accessTokenValue;
    }
}

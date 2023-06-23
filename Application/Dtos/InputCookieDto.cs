﻿using FireplaceApi.Application.Auth;
using FireplaceApi.Domain.Attributes;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Application.Dtos;

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

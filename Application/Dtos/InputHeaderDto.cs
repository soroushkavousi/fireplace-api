using FireplaceApi.Application.Auth;
using FireplaceApi.Domain.Attributes;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace FireplaceApi.Application.Dtos;

public class InputHeaderDto
{
    [Sensitive]
    public string AccessTokenValue { get; set; }
    public IPAddress IpAddress { get; set; }

    public InputHeaderDto(HttpContext httpContext)
    {
        AccessTokenValue = ExtractAccessTokenValue(httpContext);
        IpAddress = ExtractIPAddress(httpContext);
    }

    private string ExtractAccessTokenValue(HttpContext httpContext)
    {
        string accessTokenValue = null;

        var authorizationHeaderStringValues = httpContext.Request.Headers.GetValue(AuthConstants.AuthorizationHeaderKey);
        if (authorizationHeaderStringValues == default(StringValues) || authorizationHeaderStringValues.Count == 0)
            return accessTokenValue;

        var AuthorizationHeaderValue = authorizationHeaderStringValues[0].To<string>();
        var match = Regexes.AuthorizationHeaderValue.Match(AuthorizationHeaderValue);
        if (match.Success)
        {
            accessTokenValue = match.Groups[1].Value;
        }
        return accessTokenValue;
    }

    private IPAddress ExtractIPAddress(HttpContext httpContext)
    {
        var xForwardedForStringValues = httpContext.Request.Headers.GetValue(Tools.Constants.X_FORWARDED_FOR);
        if (xForwardedForStringValues == default(StringValues) || xForwardedForStringValues.Count == 0)
            return httpContext.Connection.RemoteIpAddress;

        var ipAddressString = xForwardedForStringValues[0].To<string>();
        var ipAddress = IPAddress.Parse(ipAddressString);
        return ipAddress;
    }
}

using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace FireplaceApi.Api.Controllers
{
    public class InputHeaderParameters
    {
        public string AccessTokenValue { get; set; }
        public IPAddress IpAddress { get; set; }

        public InputHeaderParameters(HttpContext httpContext)
        {
            AccessTokenValue = ExtractAccessTokenValue(httpContext);
            IpAddress = ExtractIPAddress(httpContext);
        }

        private string ExtractAccessTokenValue(HttpContext httpContext)
        {
            string accessTokenValue = null;

            var authorizationHeaderStringValues = httpContext.Request.Headers.GetValue(Tools.Constants.AuthorizationHeaderKey);
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
            return httpContext.Connection.RemoteIpAddress;
        }
    }
}

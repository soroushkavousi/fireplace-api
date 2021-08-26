using Microsoft.AspNetCore.Http;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers.Parameters
{
    public class ControllerInputHeaderParameters
    {
        public string AccessTokenValue { get; set; }
        public IPAddress IpAddress { get; set; }

        public ControllerInputHeaderParameters(HttpContext httpContext)
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

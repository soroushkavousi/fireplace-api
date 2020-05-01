using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using GamingCommunityApi.Api.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Interfaces;
using GamingCommunityApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GamingCommunityApi.Api.Tools;
using GamingCommunityApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GamingCommunityApi.Api.Controllers.Parameters.UserParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerSignUpWithGoogleInputQueryParameters
    {
        [Required]
        [FromQuery(Name = "state")]
        public string State { get; set; }

        [Required]
        [FromQuery(Name = "code")]
        public string Code { get; set; }

        [Required]
        [FromQuery(Name = "scope")]
        public string Scope { get; set; }

        [Required]
        [FromQuery(Name = "authuser")]
        public string AuthUser { get; set; }

        [Required]
        [FromQuery(Name = "prompt")]
        public string Prompt { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(State).ToSnakeCase()] = new OpenApiString("M3VuIFEwciAyNiAyMDIwIDE1Oj30IjE0IEdNVCswDNMwIChJcmFuIERheWxpZ2h0IFRabWMp"),
            [nameof(Code).ToSnakeCase()] = new OpenApiString("4/zAH66K5JT4LkfzhmImjSjCiUFQxfhOdupfa9dnt8ao0Yy4V_kVyvevHNUr5r6RM5th0MaQEzuf5ixlFIkrCVHQ0"),
            [nameof(Scope).ToSnakeCase()] = new OpenApiString("email profile https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email openid"),
            [nameof(AuthUser).ToSnakeCase()] = new OpenApiString("12345"),
            [nameof(Prompt).ToSnakeCase()] = new OpenApiString("consent"),
        };
    }

    public class ControllerSignUpWithGoogleOutputCookieParameters : IControllerOutputCookieParameters
    {
        [Required]
        public string AccessToken { get; set; }

        public ControllerSignUpWithGoogleOutputCookieParameters(string accessToken)
        {
            AccessToken = accessToken;
        }

        public CookieCollection GetCookieCollection()
        {
            var cookieCollection = new CookieCollection
            {
                new Cookie(Constants.ResponseAccessTokenCookieKey, AccessToken)
            };
            return cookieCollection;
        }
    }
}

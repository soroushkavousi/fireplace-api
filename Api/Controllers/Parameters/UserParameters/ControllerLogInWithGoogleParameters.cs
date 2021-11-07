using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerLogInWithGoogleInputQueryParameters
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

        [Required]
        [FromQuery(Name = "error")]
        public string Error { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(State).ToSnakeCase()] = new OpenApiString("M3VuIBEwciAyNiAyMDIwIDE1Oj30IjE0IEdNVCswDNMwIChJcmFuIERheWxpZ2h0IFRabWMp"),
            [nameof(Code).ToSnakeCase()] = new OpenApiString("4/zAH61K5JT4LkfzhmImjSjCiUFQxfhOdupfa9dnt8ao0Yy4V_kVyvevHNUr5r6RM5th0MaQEzuf5ixlFIkrCVHQ0"),
            [nameof(Scope).ToSnakeCase()] = new OpenApiString("email profile openid https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email"),
            [nameof(AuthUser).ToSnakeCase()] = new OpenApiString("12345"),
            [nameof(Prompt).ToSnakeCase()] = new OpenApiString("consent"),
            [nameof(Error).ToSnakeCase()] = new OpenApiString("access_denied"),
        };
    }

    public class ControllerLogInWithGoogleOutputCookieParameters : IControllerOutputCookieParameters
    {
        [Required]
        public string AccessToken { get; set; }

        public ControllerLogInWithGoogleOutputCookieParameters(string accessToken)
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

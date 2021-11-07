using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerLogInWithUsernameInputBodyParameters
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
        };
    }

    public class ControllerLogInWithUsernameOutputCookieParameters : IControllerOutputCookieParameters
    {
        [Required]
        public string AccessToken { get; set; }

        public ControllerLogInWithUsernameOutputCookieParameters(string accessToken)
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

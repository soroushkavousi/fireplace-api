using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerSignUpWithEmailInputBodyParameters
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(FirstName).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        };
    }

    public class ControllerSignUpWithEmailOutputCookieParameters : IControllerOutputCookieParameters
    {
        [Required]
        public string AccessToken { get; set; }

        public ControllerSignUpWithEmailOutputCookieParameters(string accessToken)
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

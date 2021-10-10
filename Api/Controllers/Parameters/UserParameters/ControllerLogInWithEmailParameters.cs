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
    public class ControllerLogInWithEmailInputBodyParameters
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
            [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
        };
    }

    public class ControllerLogInWithEmailOutputCookieParameters : IControllerOutputCookieParameters
    {
        [Required]
        public string AccessToken { get; set; }

        public ControllerLogInWithEmailOutputCookieParameters(string accessToken)
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

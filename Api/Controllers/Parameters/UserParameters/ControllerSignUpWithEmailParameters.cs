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

namespace GamingCommunityApi.Api.Controllers.Parameters.UserParameters
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
            [nameof(FirstName).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureEmailExample1[nameof(EmailDto.Address).ToSnakeCase()],
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

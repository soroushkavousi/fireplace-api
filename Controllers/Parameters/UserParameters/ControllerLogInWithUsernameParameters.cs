using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using GamingCommunityApi.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Interfaces;
using GamingCommunityApi.Models;
using GamingCommunityApi.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GamingCommunityApi.Tools;

namespace GamingCommunityApi.Controllers.Parameters.UserParameters
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
            [nameof(Username).ToSnakeCase()] = UserDto.UserExample1[nameof(UserDto.Username).ToSnakeCase()],
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

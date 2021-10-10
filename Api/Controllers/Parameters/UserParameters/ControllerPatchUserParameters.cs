using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchUserByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    public class ControllerPatchUserByUsernameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "username")]
        public string Username { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchUserInputBodyParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("NewFirstName"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("NewLastName"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("NewUsername"),
            [nameof(OldPassword).ToSnakeCase()] = new OpenApiString("OldPassword"),
            [nameof(Password).ToSnakeCase()] = new OpenApiString("NewPassword"),
            [nameof(EmailAddress).ToSnakeCase()] = new OpenApiString("NewEmailAddress"),
        };
    }
}

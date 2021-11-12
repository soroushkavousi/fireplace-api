using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;

namespace FireplaceApi.Api.Controllers
{
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

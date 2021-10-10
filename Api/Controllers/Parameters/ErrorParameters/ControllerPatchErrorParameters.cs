using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchErrorInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "code")]
        public int? Code { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchErrorInputBodyParameters
    {
        public string Message { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Message).ToSnakeCase()] = new OpenApiString("The new error message."),
        };
    }
}

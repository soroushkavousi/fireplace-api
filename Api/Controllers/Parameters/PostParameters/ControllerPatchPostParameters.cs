using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchPostByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchPostInputBodyParameters
    {
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = new OpenApiString("New Content"),
        };
    }
}

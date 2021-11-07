using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchCommentByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchCommentInputBodyParameters
    {
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = new OpenApiString("New Content"),
        };
    }
}

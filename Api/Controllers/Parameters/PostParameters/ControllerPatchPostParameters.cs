using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchCommunityByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchCommunityInputBodyParameters
    {
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = new OpenApiString("New Content"),
        };
    }
}

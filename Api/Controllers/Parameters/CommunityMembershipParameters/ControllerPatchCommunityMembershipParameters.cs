using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchCommunityMembershipByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchCommunityMembershipInputBodyParameters
    {
        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            ["nothing"] = new OpenApiString("nothing"),
        };
    }
}

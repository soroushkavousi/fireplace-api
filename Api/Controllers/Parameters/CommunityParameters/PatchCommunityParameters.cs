using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FireplaceApi.Api.Controllers
{
    public class PatchCommunityByEncodedIdOrNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string EncodedIdOrName { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PatchCommunityInputBodyParameters
    {
        [JsonPropertyName("name")]
        public string NewName { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            ["name"] = new OpenApiString("new-name"),
        };
    }
}

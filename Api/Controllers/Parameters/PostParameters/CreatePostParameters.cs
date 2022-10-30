using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class CreatePostInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string CommunityIdOrName { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class CreatePostInputBodyParameters
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Content).ToSnakeCase()],
        };
    }
}

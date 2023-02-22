using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
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

using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerCreatePostInputBodyParameters
    {
        public string CommunityId { get; set; }
        public string CommunityName { get; set; }
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Content).ToSnakeCase()],
        };
    }
}

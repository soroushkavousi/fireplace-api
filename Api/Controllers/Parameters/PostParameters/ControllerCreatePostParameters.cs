using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerCreatePostInputBodyParameters
    {
        [Required]
        public string Content { get; set; }
        public long CommunityId { get; set; }
        public string CommunityName { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = PostDto.PureExample2[nameof(PostDto.Content).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
        };
    }
}

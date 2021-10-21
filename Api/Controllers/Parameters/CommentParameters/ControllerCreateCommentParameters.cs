using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerReplyToPostInputBodyParameters
    {
        public long PostId { get; set; }
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(PostId).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Id).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerReplyToCommentInputBodyParameters
    {
        public long CommentId { get; set; }
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(CommentId).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Id).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }
}

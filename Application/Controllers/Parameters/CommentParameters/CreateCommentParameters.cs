using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    public class ReplyToPostInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string PostId { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ReplyToPostInputBodyParameters
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }


    public class ReplyToCommentInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ReplyToCommentInputBodyParameters
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }
}

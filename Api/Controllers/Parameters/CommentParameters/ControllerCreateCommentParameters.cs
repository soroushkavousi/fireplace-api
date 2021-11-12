using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerReplyToPostInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string PostId { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerReplyToPostInputBodyParameters
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }


    public class ControllerReplyToCommentInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerReplyToCommentInputBodyParameters
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };
    }
}

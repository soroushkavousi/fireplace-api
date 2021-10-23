using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerVoteCommentInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerVoteCommentInputBodyParameters
    {
        [Required]
        public bool? IsUpvote { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(IsUpvote).ToSnakeCase()] = new OpenApiBoolean(true),
        };
    }

    public class ControllerToggleVoteForCommentInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long Id { get; set; }
    }

    public class ControllerDeleteVoteForCommentInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long Id { get; set; }
    }
}

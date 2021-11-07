using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetCommentByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    public class ControllerGetCommentInputQueryParameters
    {
        [FromQuery(Name = "include_author")]
        public bool IncludeAuthor { get; set; } = false;

        [FromQuery(Name = "include_post")]
        public bool IncludePost { get; set; } = false;
    }
}

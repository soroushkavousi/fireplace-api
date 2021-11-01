using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetPostByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long Id { get; set; }
    }

    public class ControllerGetPostInputQueryParameters
    {
        [FromQuery(Name = "include_author")]
        public bool IncludeAuthor { get; set; } = false;

        [FromQuery(Name = "include_community")]
        public bool IncludeCommunity { get; set; } = false;
    }
}

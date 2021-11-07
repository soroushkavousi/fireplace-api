using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListSelfCommentsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        public SortType? Sort { get; set; }

        [FromQuery(Name = "sort")]
        public string StringOfSort { get; set; }
    }

    public class ControllerListPostCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "postId")]
        public string PostId { get; set; }
    }

    public class ControllerListPostCommentsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        public SortType? Sort { get; set; }

        [FromQuery(Name = "sort")]
        public string StringOfSort { get; set; }
    }

    public class ControllerListChildCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "parentId")]
        public string ParentId { get; set; }
    }
}

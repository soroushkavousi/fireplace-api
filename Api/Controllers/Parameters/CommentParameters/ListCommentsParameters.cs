using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ListSelfCommentsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        public SortType? Sort { get; set; }

        [FromQuery(Name = "sort")]
        public string StringOfSort { get; set; }
    }

    public class ListPostCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string PostId { get; set; }
    }

    public class ListPostCommentsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        public SortType? Sort { get; set; }

        [FromQuery(Name = "sort")]
        public string StringOfSort { get; set; }
    }

    public class ListChildCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string ParentId { get; set; }
    }
}

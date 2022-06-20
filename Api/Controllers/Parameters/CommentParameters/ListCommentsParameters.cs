using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ListSelfCommentsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
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
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }

    public class ListChildCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string ParentId { get; set; }
    }
}

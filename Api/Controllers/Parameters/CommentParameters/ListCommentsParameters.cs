using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ListPostCommentsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string PostId { get; set; }
    }

    public class ListPostCommentsInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }

    public class ListCommentsInputQueryParameters
    {
        [FromQuery(Name = "ids")]
        public string Ids { get; set; }

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

    public class ListSelfCommentsInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }
}

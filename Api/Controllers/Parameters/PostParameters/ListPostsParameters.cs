using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ListCommunityPostsInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string CommunityIdOrName { get; set; }
    }

    public class ListCommunityPostsInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }

    public class ListPostsInputQueryParameters
    {
        [FromQuery(Name = "search")]
        public string Search { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }

        [FromQuery(Name = "ids")]
        public string Ids { get; set; }
    }

    public class ListSelfPostsInputQueryParameters
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }
}

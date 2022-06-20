using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ListPostsInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "self")]
        public bool? Self { get; set; }

        [FromQuery(Name = "joined")]
        public bool? Joined { get; set; }

        [FromQuery(Name = "community_id")]
        public string CommunityId { get; set; }

        [FromQuery(Name = "community_name")]
        public string CommunityName { get; set; }

        [FromQuery(Name = "search")]
        public string Search { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string Sort { get; set; }
    }
}

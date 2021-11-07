using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListPostsInputQueryParameters : PaginationInputQueryParameters
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
        public SortType? Sort { get; set; }

        [FromQuery(Name = "sort")]
        public string StringOfSort { get; set; }
    }
}

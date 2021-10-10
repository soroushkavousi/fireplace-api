using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListCommunitiesInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "community_id")]
        public string CommunityId { get; set; }

        [FromQuery(Name = "community_name")]
        public string CommunityName { get; set; }
    }
}

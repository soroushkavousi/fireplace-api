using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ListCommunitiesInputQueryParameters : PaginationInputQueryParameters
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}

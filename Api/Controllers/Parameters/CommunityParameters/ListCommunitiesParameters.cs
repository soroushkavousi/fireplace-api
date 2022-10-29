using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ListCommunitiesInputQueryParameters
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(CommunitySortType))]
        public string Sort { get; set; }

        [FromQuery(Name = "ids")]
        public string Ids { get; set; }
    }
}

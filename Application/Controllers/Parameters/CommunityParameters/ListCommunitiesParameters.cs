using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Application.Controllers
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

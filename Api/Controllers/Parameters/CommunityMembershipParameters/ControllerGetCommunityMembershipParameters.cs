using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetCommunityMembershipByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    public class ControllerGetCommunityMembershipInputQueryParameters
    {
        [FromQuery(Name = "include_creator")]
        public bool IncludeCreator { get; set; } = false;

        [FromQuery(Name = "include_community")]
        public bool IncludeCommunity { get; set; } = false;
    }
}

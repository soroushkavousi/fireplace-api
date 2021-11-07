using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetCommunityByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    public class ControllerGetCommunityByNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "name")]
        public string Name { get; set; }
    }

    public class ControllerGetCommunityInputQueryParameters
    {
        [FromQuery(Name = "include_creator")]
        public bool IncludeCreator { get; set; } = true;
    }
}

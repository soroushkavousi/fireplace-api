using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetCommunityByIdOrNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string EncodedIdOrName { get; set; }
    }

    public class ControllerGetCommunityInputQueryParameters
    {
        [FromQuery(Name = "include_creator")]
        public bool IncludeCreator { get; set; } = true;
    }
}

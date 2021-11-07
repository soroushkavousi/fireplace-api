using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerDeleteCommunityByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    public class ControllerDeleteCommunityByNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "name")]
        public string Name { get; set; }
    }
}

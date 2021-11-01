using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerDeleteCommunityByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long Id { get; set; }
    }

    public class ControllerDeleteCommunityByNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "name")]
        public string Name { get; set; }
    }
}

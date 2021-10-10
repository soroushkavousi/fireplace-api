using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetEmailByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    public class ControllerGetEmailByIdInputQueryParameters
    {
        [FromQuery(Name = "include_user")]
        public bool IncludeUser { get; set; } = true;
    }
}

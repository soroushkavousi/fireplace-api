using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetSessionByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    public class ControllerGetSessionByIdInputQueryParameters
    {
        [FromQuery(Name = "include_user")]
        public bool IncludeUser { get; set; } = true;
    }
}

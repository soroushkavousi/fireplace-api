using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetUserByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    public class ControllerGetUserByUsernameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "username")]
        public string Username { get; set; }
    }

    public class ControllerGetUserInputQueryParameters
    {
        [FromQuery(Name = "include_email")]
        public bool IncludeEmail { get; set; } = true;
        [FromQuery(Name = "include_sessions")]
        public bool IncludeSessions { get; set; } = false;
    }
}

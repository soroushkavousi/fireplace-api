using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetUserByEncodedIdOrUsernameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-username")]
        public string EncodedIdOrUsername { get; set; }
    }

    public class ControllerGetUserInputQueryParameters
    {
        [FromQuery(Name = "include_email")]
        public bool IncludeEmail { get; set; } = true;
        [FromQuery(Name = "include_sessions")]
        public bool IncludeSessions { get; set; } = false;
    }
}

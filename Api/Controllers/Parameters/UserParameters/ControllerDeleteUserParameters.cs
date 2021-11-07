using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerDeleteUserByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }

    public class ControllerDeleteUserByUsernameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "username")]
        public string Username { get; set; }
    }
}

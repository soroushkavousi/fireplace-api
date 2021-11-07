using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerRevokeSessionInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }
}

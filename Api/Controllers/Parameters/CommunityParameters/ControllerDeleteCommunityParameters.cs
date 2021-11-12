using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerDeleteCommunityByEncodedIdOrNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string EncodedIdOrName { get; set; }
    }
}

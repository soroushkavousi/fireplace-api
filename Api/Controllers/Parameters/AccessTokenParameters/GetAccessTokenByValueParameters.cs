using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class GetAccessTokenByValueInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "value")]
        public string Value { get; set; }
    }

    public class GetAccessTokenByValueInputQueryParameters
    {
        [FromQuery(Name = "include_user")]
        public bool IncludeUser { get; set; } = true;
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class GetErrorByCodeInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "code")]
        public int Code { get; set; }
    }
}

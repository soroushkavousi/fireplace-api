using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetErrorByCodeInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "code")]
        public int? Code { get; set; }
    }

    [BindNever]
    public class ControllerGetErrorByCodeInputQueryParameters
    {

    }
}

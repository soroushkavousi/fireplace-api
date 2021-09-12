using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerGetAccessTokenByValueInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "value")]
        public string Value { get; set; }
    }

    public class ControllerGetAccessTokenByValueInputQueryParameters
    {
        [FromQuery(Name = "include_user")]
        public bool IncludeUser { get; set; } = true;
    }
}

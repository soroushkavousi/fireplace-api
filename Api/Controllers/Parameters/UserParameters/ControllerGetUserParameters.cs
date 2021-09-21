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
    public class ControllerGetUserByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
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

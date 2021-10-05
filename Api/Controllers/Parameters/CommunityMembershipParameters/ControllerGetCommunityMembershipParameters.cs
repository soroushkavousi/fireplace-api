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
    public class ControllerGetCommunityMembershipByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    public class ControllerGetCommunityMembershipInputQueryParameters
    {
        [FromQuery(Name = "include_creator")]
        public bool IncludeCreator { get; set; } = false;

        [FromQuery(Name = "include_community")]
        public bool IncludeCommunity { get; set; } = false;
    }
}

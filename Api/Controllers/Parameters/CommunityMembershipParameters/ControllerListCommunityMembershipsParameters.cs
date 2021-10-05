using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListCommunityMembershipsInputQueryParameters : PaginationInputQueryParameters
    {
        //[FromQuery(Name = "user_id")]
        //public long? UserId { get; set; }

        //[FromQuery(Name = "username")]
        //public string Username { get; set; }

        //[FromQuery(Name = "community_id")]
        //public long? CommunityId { get; set; }

        //[FromQuery(Name = "community_name")]
        //public string CommunityName { get; set; }
    }
}

using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerDeleteCommunityMembershipByCommunityIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "communityId")]
        public string CommunityId { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerDeleteCommunityMembershipByCommunityNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "communityName")]
        public string CommunityName { get; set; }
    }
}

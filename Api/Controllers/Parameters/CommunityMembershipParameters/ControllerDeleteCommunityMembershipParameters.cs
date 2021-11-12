using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerDeleteCommunityMembershipByCommunityEncodedIdOrNameInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string CommunityEncodedIdOrName { get; set; }
    }
}

using FireplaceApi.Application.Tools;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class DeleteCommunityMembershipByCommunityIdentifierInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string CommunityEncodedIdOrName { get; set; }
    }
}

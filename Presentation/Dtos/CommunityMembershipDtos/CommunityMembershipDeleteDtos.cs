using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class DeleteCommunityMembershipByCommunityIdentifierInputRouteDto
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string CommunityEncodedIdOrName { get; set; }
}

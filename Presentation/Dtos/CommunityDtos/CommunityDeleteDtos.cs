using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class DeleteCommunityByEncodedIdOrNameInputRouteDto
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string EncodedIdOrName { get; set; }
}

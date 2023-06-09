using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/community-memberships")]
[Produces("application/json")]
public class CommunityMembershipController : ApiController
{
    private readonly CommunityMembershipService _communityMembershipService;

    public CommunityMembershipController(CommunityMembershipService communityMembershipService)
    {
        _communityMembershipService = communityMembershipService;
    }

    /// <summary>
    /// Create a community membership.
    /// </summary>
    /// <returns>Created community membership</returns>
    /// <response code="200">Returns the newly created item</response>
    [HttpPost("/v{version:apiVersion}/communities/{id-or-name}/members")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CommunityMembershipDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CommunityMembershipDto>> CreateCommunityMembershipAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] CreateCommunityMembershipInputRouteDto inputRouteDto)
    {
        var communityMembership = await _communityMembershipService.CreateCommunityMembershipAsync(
            requestingUser, inputRouteDto.CommunityIdentifier);
        var communityMembershipDto = communityMembership.ToDto();
        return communityMembershipDto;
    }

    /// <summary>
    /// Leave a community.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The community membership was successfully deleted.</response>
    [HttpDelete("/v{version:apiVersion}/communities/{id-or-name}/members/me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCommunityMembershipByCommunityIdentifierAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] DeleteCommunityMembershipByCommunityIdentifierInputRouteDto inputRouteDto)
    {
        await _communityMembershipService.DeleteCommunityMembershipAsync(requestingUser,
            inputRouteDto.CommunityIdentifier);
        return Ok();
    }
}

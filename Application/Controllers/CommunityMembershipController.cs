using FireplaceApi.Application.Auth;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommunityMembershipDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("/v{version:apiVersion}/communities/{id-or-name}/members")]
    public async Task<ActionResult<CommunityMembershipDto>> CreateCommunityMembershipAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] CreateCommunityMembershipInputRouteDto inputRouteDto)
    {
        var communityMembership = await _communityMembershipService.CreateCommunityMembershipAsync(
            requestingUser.Id.Value, inputRouteDto.CommunityIdentifier);
        var communityMembershipDto = communityMembership.ToDto();
        return communityMembershipDto;
    }

    /// <summary>
    /// Leave a community.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The community membership was successfully deleted.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("/v{version:apiVersion}/communities/{id-or-name}/members/me")]
    public async Task<IActionResult> DeleteCommunityMembershipByCommunityIdentifierAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeleteCommunityMembershipByCommunityIdentifierInputRouteDto inputRouteDto)
    {
        await _communityMembershipService.DeleteCommunityMembershipAsync(requestingUser.Id.Value,
            inputRouteDto.CommunityIdentifier);
        return Ok();
    }
}

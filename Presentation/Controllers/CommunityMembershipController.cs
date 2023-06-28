using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/community-memberships")]
[Produces("application/json")]
public class CommunityMembershipController : ApiController
{
    private readonly ISender _sender;

    public CommunityMembershipController(ISender sender)
    {
        _sender = sender;
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
        var communityIdentifier = inputRouteDto.CommunityEncodedIdOrName.ToCommunityIdentifier();
        var command = new JoinCommunityCommand(requestingUser.Id.Value, communityIdentifier);
        var communityMembership = await _sender.Send(command);
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
        var communityIdentifier = inputRouteDto.CommunityEncodedIdOrName.ToCommunityIdentifier();
        var command = new LeaveCommunityCommand(requestingUser.Id.Value, communityIdentifier);
        await _sender.Send(command);
        return Ok();
    }
}

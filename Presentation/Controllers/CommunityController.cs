using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/communities")]
[Produces("application/json")]
public class CommunityController : ApiController
{
    private readonly ISender _sender;

    public CommunityController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// List communities.
    /// </summary>
    /// <returns>List of communities</returns>
    /// <response code="200">The communities was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<CommunityDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<QueryResultDto<CommunityDto>>> ListCommunitiesAsync(
        [FromQuery] ListCommunitiesInputQueryDto inputQueryDto)
    {
        var queryResult = new QueryResult<Community>(null, null);
        if (!inputQueryDto.EncodedIds.IsNullOrEmpty())
        {
            var ids = inputQueryDto.EncodedIds.ToIds(FieldName.COMMUNITY_ID);
            var query = new ListCommunitiesByIdsQuery(ids);
            queryResult.Items = await _sender.Send(query);
        }
        else
        {
            var sort = inputQueryDto.SortString.ToNullableEnum<CommunitySortType>();
            var query = new SearchCommunitiesQuery(inputQueryDto.Search, sort);
            queryResult = await _sender.Send(query);
        }
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// List joined communities.
    /// </summary>
    /// <returns>List of communities</returns>
    /// <response code="200">The communities was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(QueryResultDto<CommunityDto>), StatusCodes.Status200OK)]
    [HttpGet("joined")]
    public async Task<ActionResult<QueryResultDto<CommunityDto>>> ListJoinedCommunitiesAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] ListJoinedCommunitiesInputQueryDto inputQueryDto)
    {
        var sort = inputQueryDto.SortString.ToNullableEnum<CommunitySortType>();
        var query = new ListJoinedCommunitiesQuery(requestingUser.Id.Value, sort);
        var queryResult = await _sender.Send(query);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// Get a single community by id or name.
    /// </summary>
    /// <returns>Requested community</returns>
    /// <response code="200">The community was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
    [HttpGet("{id-or-name}")]
    public async Task<ActionResult<CommunityDto>> GetCommunityByIdOrNameAsync(
        [FromRoute] GetCommunityByIdOrNameInputRouteDto inputRouteDto)
    {
        var communityIdentifier = inputRouteDto.EncodedIdOrName.ToCommunityIdentifier();
        var query = new GetCommunityByIdOrNameQuery(communityIdentifier);
        var community = await _sender.Send(query);
        var communityDto = community.ToDto();
        return communityDto;
    }

    /// <summary>
    /// Create a community.
    /// </summary>
    /// <returns>Created community</returns>
    /// <response code="200">Returns the newly created item</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost]
    public async Task<ActionResult<CommunityDto>> CreateCommunityAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] CreateCommunityInputBodyDto inputBodyDto)
    {
        var command = new CreateCommunityCommand(requestingUser.Id.Value, inputBodyDto.Name);
        var community = await _sender.Send(command);
        var communityDto = community.ToDto();
        return communityDto;
    }

    /// <summary>
    /// Update a single community by id or name.
    /// </summary>
    /// <returns>Updated community</returns>
    /// <response code="200">The community was successfully updated.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("{id-or-name}")]
    public async Task<ActionResult<CommunityDto>> PatchCommunityByEncodedIdOrNameAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] PatchCommunityByEncodedIdOrNameInputRouteDto inputRouteDto,
        [FromBody] PatchCommunityInputBodyDto inputBodyDto)
    {
        var communityIdentifier = inputRouteDto.EncodedIdOrName.ToCommunityIdentifier();
        var communityNewName = new CommunityName(inputBodyDto.NewName);
        var command = new PatchCommunityCommand(requestingUser.Id.Value, communityIdentifier, communityNewName);
        var community = await _sender.Send(command);
        var communityDto = community.ToDto();
        return communityDto;
    }

    /// <summary>
    /// Delete a single community by id or name.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The community was successfully deleted.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id-or-name}")]
    public async Task<IActionResult> DeleteCommunityByIdOrNameAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeleteCommunityByEncodedIdOrNameInputRouteDto inputRouteDto)
    {
        var communityIdentifier = inputRouteDto.EncodedIdOrName.ToCommunityIdentifier();
        var command = new DeleteCommunityCommand(requestingUser.Id.Value, communityIdentifier);
        await _sender.Send(command);
        return Ok();
    }
}

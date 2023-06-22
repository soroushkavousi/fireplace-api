using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Services;
using FireplaceApi.Application.ValueObjects;
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
    private readonly CommunityService _communityService;

    public CommunityController(CommunityService communityService)
    {
        _communityService = communityService;
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
            queryResult.Items = await _communityService.ListCommunitiesByIdsAsync(
                inputQueryDto.Ids);
        }
        else
        {
            queryResult = await _communityService.ListCommunitiesAsync(inputQueryDto.Search,
                inputQueryDto.Sort);
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
        var queryResult = await _communityService.ListJoinedCommunitiesAsync(requestingUser.Id.Value,
            inputQueryDto.Sort);
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
        var community = await _communityService.GetCommunityByIdentifierAsync(
            inputRouteDto.Identifier);
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
        var community = await _communityService.CreateCommunityAsync(requestingUser.Id.Value,
            inputBodyDto.Name);
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
        var community = await _communityService.PatchCommunityByIdentifierAsync(requestingUser.Id.Value,
            inputRouteDto.Identifier, inputBodyDto.NewName);
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
        await _communityService.DeleteCommunityByIdentifierAsync(requestingUser.Id.Value,
            inputRouteDto.Identifier);
        return Ok();
    }
}

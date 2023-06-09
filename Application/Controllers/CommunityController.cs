using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

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
    [HttpGet]
    [ProducesResponseType(typeof(QueryResultDto<CommunityDto>), StatusCodes.Status200OK)]
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
    [HttpGet("joined")]
    [ProducesResponseType(typeof(QueryResultDto<CommunityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResultDto<CommunityDto>>> ListJoinedCommunitiesAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromQuery] ListJoinedCommunitiesInputQueryDto inputQueryDto)
    {
        var queryResult = await _communityService.ListJoinedCommunitiesAsync(requestingUser,
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
    [HttpGet("{id-or-name}")]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
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
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CommunityDto>> CreateCommunityAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromBody] CreateCommunityInputBodyDto inputBodyDto)
    {
        var community = await _communityService.CreateCommunityAsync(requestingUser,
            inputBodyDto.Name);
        var communityDto = community.ToDto();
        return communityDto;
    }

    /// <summary>
    /// Update a single community by id or name.
    /// </summary>
    /// <returns>Updated community</returns>
    /// <response code="200">The community was successfully updated.</response>
    [HttpPatch("{id-or-name}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CommunityDto>> PatchCommunityByEncodedIdOrNameAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] PatchCommunityByEncodedIdOrNameInputRouteDto inputRouteDto,
        [FromBody] PatchCommunityInputBodyDto inputBodyDto)
    {
        var community = await _communityService.PatchCommunityByIdentifierAsync(requestingUser,
            inputRouteDto.Identifier, inputBodyDto.NewName);
        var communityDto = community.ToDto();
        return communityDto;
    }

    /// <summary>
    /// Delete a single community by id or name.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The community was successfully deleted.</response>
    [HttpDelete("{id-or-name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCommunityByIdOrNameAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] DeleteCommunityByEncodedIdOrNameInputRouteDto inputRouteDto)
    {
        await _communityService.DeleteCommunityByIdentifierAsync(requestingUser, inputRouteDto.Identifier);
        return Ok();
    }
}

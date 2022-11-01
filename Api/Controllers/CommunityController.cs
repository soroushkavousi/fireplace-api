using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using FireplaceApi.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/communities")]
    [Produces("application/json")]
    public class CommunityController : ApiController
    {
        private readonly ILogger<CommunityController> _logger;
        private readonly CommunityConverter _communityConverter;
        private readonly CommunityService _communityService;

        public CommunityController(ILogger<CommunityController> logger,
            CommunityConverter communityConverter, CommunityService communityService)
        {
            _logger = logger;
            _communityConverter = communityConverter;
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
            [BindNever][FromHeader] User requestingUser,
            [FromQuery] ListCommunitiesInputQueryParameters inputQueryParameters)
        {
            var queryResult = new QueryResult<Community>(null, null);
            if (!inputQueryParameters.Ids.IsNullOrEmpty())
            {
                queryResult.Items = await _communityService.ListCommunitiesByIdsAsync(
                    inputQueryParameters.Ids);
            }
            else
            {
                queryResult = await _communityService.ListCommunitiesAsync(inputQueryParameters.Name,
                    inputQueryParameters.Sort);
            }
            var queryResultDto = _communityConverter.ConvertToDto(queryResult);
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
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] GetCommunityByIdOrNameInputRouteParameters inputRouteParameters,
            [FromQuery] GetCommunityInputQueryParameters inputQueryParameters)
        {
            var community = await _communityService
                .GetCommunityByEncodedIdOrNameAsync(requestingUser, inputRouteParameters.EncodedIdOrName,
                inputQueryParameters.IncludeCreator);
            var communityDto = _communityConverter.ConvertToDto(community);
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
            [FromBody] CreateCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.CreateCommunityAsync(requestingUser,
                inputBodyParameters.Name);
            var communityDto = _communityConverter.ConvertToDto(community);
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
            [FromRoute] PatchCommunityByEncodedIdOrNameInputRouteParameters inputRouteParameters,
            [FromBody] PatchCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.PatchCommunityByEncodedIdOrNameAsync(requestingUser,
                inputRouteParameters.EncodedIdOrName, inputBodyParameters.NewName);
            var communityDto = _communityConverter.ConvertToDto(community);
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
            [FromRoute] DeleteCommunityByEncodedIdOrNameInputRouteParameters inputRouteParameters)
        {
            await _communityService.DeleteCommunityByEncodedIdOrNameAsync(requestingUser, inputRouteParameters.EncodedIdOrName);
            return Ok();
        }
    }
}

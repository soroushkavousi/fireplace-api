using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
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
        /// List all communities.
        /// </summary>
        /// <returns>List of communities</returns>
        /// <response code="200">All communities was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(PageDto<CommunityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommunityDto>>> ListCommunitiesAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListCommunitiesInputQueryParameters inputQueryParameters)
        {
            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _communityService.ListCommunitiesAsync(requesterUser,
                paginationInputParameters, inputQueryParameters.Name);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _communityConverter.ConvertToDto(page, requestPath);
            //SetOutputHeaderParameters(communityDtos.HeaderParameters);
            return pageDto;
        }

        /// <summary>
        /// Get a single community by id.
        /// </summary>
        /// <returns>Requested community</returns>
        /// <response code="200">The community was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> GetCommunityByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommunityByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommunityInputQueryParameters inputQueryParameters)
        {
            var community = await _communityService
                .GetCommunityByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeCreator);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Get a single community by name.
        /// </summary>
        /// <returns>Requested community</returns>
        /// <response code="200">The community was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> GetCommunityByNameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommunityByNameInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommunityInputQueryParameters inputQueryParameters)
        {
            var community = await _communityService
                .GetCommunityByNameAsync(requesterUser, inputRouteParameters.Name,
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
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerCreateCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.CreateCommunityAsync(requesterUser,
                inputBodyParameters.Name);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Update a single community by id.
        /// </summary>
        /// <returns>Updated community</returns>
        /// <response code="200">The community was successfully updated.</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> PatchCommunityByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchCommunityByIdInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.PatchCommunityByIdAsync(requesterUser,
                inputRouteParameters.Id, inputBodyParameters.NewName);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Update a single community by name.
        /// </summary>
        /// <returns>Updated community</returns>
        /// <response code="200">The community was successfully updated.</response>
        [HttpPatch("{name}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> PatchCommunityByNameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchCommunityByNameInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.PatchCommunityByNameAsync(requesterUser,
                inputRouteParameters.Name, inputBodyParameters.NewName);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Delete a single community by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community was successfully deleted.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityByIdInputRouteParameters inputRouteParameters)
        {
            await _communityService.DeleteCommunityByIdAsync(requesterUser, inputRouteParameters.Id);
            return Ok();
        }

        /// <summary>
        /// Delete a single community by name.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community was successfully deleted.</response>
        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityByNameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityByNameInputRouteParameters inputRouteParameters)
        {
            await _communityService.DeleteCommunityByNameAsync(requesterUser, inputRouteParameters.Name);
            return Ok();
        }
    }
}

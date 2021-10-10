using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/community-memberships")]
    [Produces("application/json")]
    public class CommunityMembershipController : ApiController
    {
        private readonly ILogger<CommunityMembershipController> _logger;
        private readonly CommunityMembershipConverter _communityMembershipConverter;
        private readonly CommunityMembershipService _communityMembershipService;

        public CommunityMembershipController(ILogger<CommunityMembershipController> logger,
            CommunityMembershipConverter communityMembershipConverter,
            CommunityMembershipService communityMembershipService)
        {
            _logger = logger;
            _communityMembershipConverter = communityMembershipConverter;
            _communityMembershipService = communityMembershipService;
        }

        /// <summary>
        /// List all community memberships.
        /// </summary>
        /// <returns>List of community memberships</returns>
        /// <response code="200">All community memberships was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PageDto<CommunityMembershipDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommunityMembershipDto>>> ListCommunityMembershipsAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListCommunityMembershipsInputQueryParameters inputQueryParameters)
        {
            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _communityMembershipService.ListCommunityMembershipsAsync(requesterUser,
                paginationInputParameters);
            var pageDto = _communityMembershipConverter.ConvertToDto(page, "/community-memberships");
            //SetOutputHeaderParameters(communityMembershipDtos.HeaderParameters);
            return pageDto;
        }

        /// <summary>
        /// Get a single community membership by id.
        /// </summary>
        /// <returns>Requested community membership</returns>
        /// <response code="200">The community membership was successfully retrieved.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(CommunityMembershipDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityMembershipDto>> GetCommunityMembershipByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommunityMembershipByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommunityMembershipInputQueryParameters inputQueryParameters)
        {
            var communityMembership = await _communityMembershipService
                .GetCommunityMembershipByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeCreator, inputQueryParameters.IncludeCommunity);
            var communityMembershipDto = _communityMembershipConverter.ConvertToDto(communityMembership);
            return communityMembershipDto;
        }

        /// <summary>
        /// Create a community membership.
        /// </summary>
        /// <returns>Created community membership</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommunityMembershipDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityMembershipDto>> CreateCommunityMembershipAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerCreateCommunityMembershipInputBodyParameters inputBodyParameters)
        {
            var communityMembership = await _communityMembershipService.CreateCommunityMembershipAsync(
                requesterUser, inputBodyParameters.CommunityId, inputBodyParameters.CommunityName);
            var communityMembershipDto = _communityMembershipConverter.ConvertToDto(communityMembership);
            return communityMembershipDto;
        }

        /// <summary>
        /// Update a single community membership by id.
        /// </summary>
        /// <returns>Updated community membership</returns>
        /// <response code="200">The community membership was successfully updated.</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(CommunityMembershipDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityMembershipDto>> PatchCommunityMembershipByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchCommunityMembershipByIdInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchCommunityMembershipInputBodyParameters inputBodyParameters)
        {
            var communityMembership = await _communityMembershipService.PatchCommunityMembershipByIdAsync(requesterUser,
                inputRouteParameters.Id);
            var communityMembershipDto = _communityMembershipConverter.ConvertToDto(communityMembership);
            return communityMembershipDto;
        }

        /// <summary>
        /// Delete a single community membership by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community membership was successfully deleted.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityMembershipByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityMembershipByIdInputRouteParameters inputRouteParameters)
        {
            await _communityMembershipService.DeleteCommunityMembershipByIdAsync(requesterUser,
                inputRouteParameters.Id);
            return Ok();
        }
    }
}

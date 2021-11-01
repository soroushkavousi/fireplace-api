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
        /// Exit from a community.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community membership was successfully deleted.</response>
        [HttpDelete("/v{version:apiVersion}/communities/{communityId:long}/members/me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityMembershipByCommunityIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityMembershipByCommunityIdInputRouteParameters inputRouteParameters)

        {
            await _communityMembershipService.DeleteCommunityMembershipAsync(requesterUser,
                inputRouteParameters.CommunityId, null);
            return Ok();
        }

        /// <summary>
        /// Exit from a community.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community membership was successfully deleted.</response>
        [HttpDelete("/v{version:apiVersion}/communities/{communityName:alpha}/members/me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityMembershipByCommunityNameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityMembershipByCommunityNameInputRouteParameters inputRouteParameters)

        {
            await _communityMembershipService.DeleteCommunityMembershipAsync(requesterUser,
                null, inputRouteParameters.CommunityName);
            return Ok();
        }
    }
}

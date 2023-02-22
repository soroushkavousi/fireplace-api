using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
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
            [BindNever][FromHeader] User requestingUser,
            [FromBody] CreateCommunityMembershipInputBodyParameters inputBodyParameters)
        {
            var communityMembership = await _communityMembershipService.CreateCommunityMembershipAsync(
                requestingUser, inputBodyParameters.CommunityId, inputBodyParameters.CommunityName);
            var communityMembershipDto = _communityMembershipConverter.ConvertToDto(communityMembership);
            return communityMembershipDto;
        }

        /// <summary>
        /// Exit from a community.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community membership was successfully deleted.</response>
        [HttpDelete("/v{version:apiVersion}/communities/{id-or-name}/members/me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityMembershipByCommunityIdentifierAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] DeleteCommunityMembershipByCommunityIdentifierInputRouteParameters inputRouteParameters)
        {
            await _communityMembershipService.DeleteCommunityMembershipAsync(requestingUser,
                inputRouteParameters.CommunityEncodedIdOrName);
            return Ok();
        }
    }
}

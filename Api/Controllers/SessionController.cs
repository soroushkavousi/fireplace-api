using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/sessions")]
    [Produces("application/json")]
    public class SessionController : ApiController
    {
        private readonly SessionConverter _sessionConverter;
        private readonly SessionService _sessionService;

        public SessionController(SessionConverter sessionConverter, SessionService sessionService)
        {
            _sessionConverter = sessionConverter;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Revoke session or session.
        /// </summary>
        /// <returns>Created basic authentication</returns>
        /// <response code="200">Returns the newly registered session.</response>
        [HttpPost("{id}/revoke")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeSession(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerRevokeSessionInputRouteParameters inputRouteParameters)
        {
            await _sessionService.RevokeSessionByIdAsync(requesterUser, inputRouteParameters.Id);
            return Ok();
        }

        /// <summary>
        /// List all sessions.
        /// </summary>
        /// <returns>List of sessions</returns>
        /// <response code="200">All sessions was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> ListSessionsAsync(
            [BindNever][FromHeader] User requesterUser)
        {
            var sessions = await _sessionService.ListSessionsAsync(requesterUser);
            var sessionDtos = sessions.Select(session => _sessionConverter.ConvertToDto(session)).ToList();
            //SetOutputHeaderParameters(sessionDtos.HeaderParameters);
            return sessionDtos;
        }

        /// <summary>
        /// Get a single session.
        /// </summary>
        /// <returns>Requested session</returns>
        /// <response code="200">The session was successfully retrieved.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SessionDto>> GetSessionByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetSessionByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetSessionByIdInputQueryParameters inputQueryParameters)
        {
            var session = await _sessionService.GetSessionByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeUser);
            var sessionDto = _sessionConverter.ConvertToDto(session);
            return sessionDto;
        }
    }
}

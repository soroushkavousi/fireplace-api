using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
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
        /// List all sessions.
        /// </summary>
        /// <returns>List of sessions</returns>
        /// <response code="200">All sessions was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> ListSessionsAsync(
            [BindNever][FromHeader] User requestingUser)
        {
            var sessions = await _sessionService.ListSessionsAsync(requestingUser);
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
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] GetSessionByIdInputRouteParameters inputRouteParameters,
            [FromQuery] GetSessionByIdInputQueryParameters inputQueryParameters)
        {
            var session = await _sessionService.GetSessionByIdAsync(requestingUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeUser);
            var sessionDto = _sessionConverter.ConvertToDto(session);
            return sessionDto;
        }


        /// <summary>
        /// Revoke session or session.
        /// </summary>
        /// <returns>Created basic authentication</returns>
        /// <response code="200">Returns the newly registered session.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeSession(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] RevokeSessionInputRouteParameters inputRouteParameters)
        {
            await _sessionService.RevokeSessionByIdAsync(requestingUser, inputRouteParameters.Id);
            return Ok();
        }
    }
}

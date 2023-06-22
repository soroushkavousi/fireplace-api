using FireplaceApi.Application.Sessions;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/sessions")]
[Produces("application/json")]
public class SessionController : ApiController
{
    private readonly SessionService _sessionService;

    public SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    /// <summary>
    /// List all sessions.
    /// </summary>
    /// <returns>List of sessions</returns>
    /// <response code="200">All sessions was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessionDto>>> ListSessionsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser)
    {
        var sessions = await _sessionService.ListSessionsAsync(requestingUser.Id.Value);
        var sessionDtos = sessions.Select(session => session.ToDto()).ToList();
        //SetOutputHeaderDto(sessionDtos.HeaderDto);
        return sessionDtos;
    }

    /// <summary>
    /// Revoke session or session.
    /// </summary>
    /// <returns>Created basic authentication</returns>
    /// <response code="200">Returns the newly registered session.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RevokeSession(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] RevokeSessionInputRouteDto inputRouteDto)
    {
        await _sessionService.RevokeSessionByIdAsync(requestingUser.Id.Value, inputRouteDto.Id);
        return Ok();
    }
}

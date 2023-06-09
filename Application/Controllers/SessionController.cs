using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionDto>>> ListSessionsAsync(
        [BindNever][FromHeader] User requestingUser)
    {
        var sessions = await _sessionService.ListSessionsAsync(requestingUser);
        var sessionDtos = sessions.Select(session => session.ToDto()).ToList();
        //SetOutputHeaderDto(sessionDtos.HeaderDto);
        return sessionDtos;
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
        [FromRoute] RevokeSessionInputRouteDto inputRouteDto)
    {
        await _sessionService.RevokeSessionByIdAsync(requestingUser, inputRouteDto.Id);
        return Ok();
    }
}

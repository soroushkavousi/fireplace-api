using FireplaceApi.Application.Auth;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

[ApiController]
[ApiVersion("1.0", Deprecated = false)]
[Route("v{version:apiVersion}/errors")]
[Produces("application/json")]
public class ErrorController : ApiController
{
    private readonly ErrorService _errorService;

    public ErrorController(ErrorService errorService)
    {
        _errorService = errorService;
    }

    /// <summary>
    /// Get a single error.
    /// </summary>
    /// <returns>Requested error</returns>
    /// <response code="200">The error was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.ADMIN))]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
    [HttpGet("{code}")]
    public async Task<ActionResult<ErrorDto>> GetErrorByCodeAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] GetErrorByCodeInputRouteDto inputRouteDto)
    {
        var error = await _errorService.GetErrorAsync(requestingUser.Id.Value, inputRouteDto.Identifier);
        var errorDto = error.ToDto();
        return errorDto;
    }
}

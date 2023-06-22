using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

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

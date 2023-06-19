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
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/emails")]
[Produces("application/json")]
public class EmailController : ApiController
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Activate requesting user email.
    /// </summary>
    /// <returns>Created basic authentication</returns>
    /// <response code="200">Returns the newly registered email.</response>
    [Authorize(Policy = AuthConstants.UnverifiedUserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("me/activate")]
    public async Task<ActionResult<EmailDto>> ActivateRequestingUserEmailAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] ActivateRequestingUserEmailInputBodyDto inputBodyDto)
    {
        var email = await _emailService.ActivateRequestingUserEmailAsync(
            requestingUser.Id.Value,
            inputBodyDto.ActivationCode.Value);

        var futureRequestingUser = requestingUser with { State = email.User.State };
        await HttpContext.SignInWithCookieAsync(futureRequestingUser);

        var emailDto = email.ToDto();
        return emailDto;
    }

    /// <summary>
    /// Resend activation code.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">New activation code successfully sent.</response>
    [Authorize(Policy = AuthConstants.UnverifiedUserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("me/resend-activation-code")]
    public async Task<ActionResult> ResendActivationCodeAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] ResendActivationCodeAsyncInputBodyDto inputBodyDto)
    {
        await _emailService.ResendActivationCodeAsync(requestingUser.Id.Value);
        return Ok();
    }

    /// <summary>
    /// Get requesting user email data.
    /// </summary>
    /// <returns>Requested email</returns>
    /// <response code="200">The email was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
    [HttpGet("me")]
    public async Task<ActionResult<EmailDto>> GetRequestingUserEmailAsync(
        [BindNever][FromHeader] RequestingUser requestingUser)
    {
        var email = await _emailService.GetRequestingUserEmailAsync(requestingUser.Id.Value);
        var emailDto = email.ToDto();
        return emailDto;
    }

    /// <summary>
    /// Change the requesting user email address
    /// </summary>
    /// <returns>Updated email</returns>
    /// <response code="200">Returns the updated email.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("me")]
    public async Task<ActionResult<EmailDto>> PatchEmailAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] PatchEmailInputBodyDto inputBodyDto)
    {
        var email = await _emailService.PatchEmailAsync(requestingUser.Id.Value,
            inputBodyDto.NewAddress);
        var emailDto = email.ToDto();
        return emailDto;
    }
}

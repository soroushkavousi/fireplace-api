using FireplaceApi.Application.Attributes;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/users")]
[Produces("application/json")]
public class UserController : ApiController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Redirect to the google-log-in page.
    /// </summary>
    /// <returns></returns>
    /// <response code="308">It redirects you to the google log in page.
    /// 
    /// However, in the swagger UI, the execute button will open a new tab before the redirection.
    /// </response>
    [AllowAnonymous]
    [HttpGet("open-google-log-in-page")]
    [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
    public async Task<ActionResult> OpenGoogleLogInPage(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto)
    {
        var googleLogInPageUrl = await _userService.GetGoogleAuthUrlAsync(inputHeaderDto.IpAddress);
        return Redirect(googleLogInPageUrl);
    }

    /// <summary>
    /// Sign up with email.
    /// </summary>
    /// <returns>Created user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [HttpPost("sign-up-with-email")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> SignUpWithEmailAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] SignUpWithEmailInputBodyDto inputBodyDto)
    {
        var user = await _userService.SignUpWithEmailAsync(inputHeaderDto.IpAddress,
            inputBodyDto.EmailAddress, inputBodyDto.Username, inputBodyDto.PasswordValueObject);
        var userDto = user.ToDto();
        var outputCookieDto = new SignUpWithEmailOutputCookieDto(userDto.AccessToken);
        SetOutputCookieDto(outputCookieDto);
        return userDto;
    }

    /// <summary>
    /// Log in or sign up with google.
    /// </summary>
    /// <returns>Created user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [ProducesCsrfToken]
    [HttpGet("log-in-with-google")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> LogInWithGoogleAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromQuery] LogInWithGoogleInputQueryDto inputQueryDto)
    {
        var user = await _userService.LogInWithGoogleAsync(inputHeaderDto.IpAddress,
            inputQueryDto.State, inputQueryDto.Code, inputQueryDto.Scope,
            inputQueryDto.AuthUser, inputQueryDto.Prompt, inputQueryDto.Error);
        var userDto = user.ToDto();
        var outputCookieDto = new SignUpWithEmailOutputCookieDto(userDto?.AccessToken);
        SetOutputCookieDto(outputCookieDto);
        if (string.IsNullOrWhiteSpace(user.GoogleUser.RedirectToUserUrl))
            return userDto;
        else
            return Redirect(user.GoogleUser.RedirectToUserUrl);
    }

    /// <summary>
    /// Log in with email.
    /// </summary>
    /// <returns>Logged in user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [HttpPost("log-in-with-email")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> LogInWithEmailAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] LogInWithEmailInputBodyDto inputBodyDto)
    {
        var user = await _userService.LogInWithEmailAsync(inputHeaderDto.IpAddress,
            inputBodyDto.EmailAddress, inputBodyDto.PasswordValueObject);
        var userDto = user.ToDto();
        var outputCookieDto = new SignUpWithEmailOutputCookieDto(userDto.AccessToken);
        SetOutputCookieDto(outputCookieDto);
        return userDto;
    }

    /// <summary>
    /// Log in with username.
    /// </summary>
    /// <returns>Logged in user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [HttpPost("log-in-with-username")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> LogInWithUsernameAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] LogInWithUsernameInputBodyDto inputBodyDto)
    {
        var user = await _userService.LogInWithUsernameAsync(inputHeaderDto.IpAddress,
            inputBodyDto.Username, inputBodyDto.PasswordValueObject);
        var userDto = user.ToDto();
        var outputCookieDto = new SignUpWithEmailOutputCookieDto(userDto.AccessToken);
        SetOutputCookieDto(outputCookieDto);
        return userDto;
    }

    /// <summary>
    /// Get the requesting user data.
    /// </summary>
    /// <returns>Requested user</returns>
    /// <response code="200">The user was successfully retrieved.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> GetRequestingUserAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromQuery] GetUserInputQueryDto inputQueryDto)
    {
        var user = await _userService.GetRequestingUserAsync(requestingUser,
            inputQueryDto.IncludeEmail, inputQueryDto.IncludeSessions);
        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Get the profile of a user
    /// </summary>
    /// <returns>Requested user profile</returns>
    /// <response code="200">The user profile was successfully retrieved.</response>
    [HttpGet("{id-or-username}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfileDto>> GetUserProfileAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] GetUserProfileInputRouteDto inputRouteDto)
    {
        var profile = await _userService.GetUserProfileAsync(requestingUser,
            inputRouteDto.Identifier);
        var profileDto = profile.ToDto();
        return profileDto;
    }

    /// <summary>
    /// Create a password for requesting user.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user password successfully set.</response>
    [HttpPost("me/password")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRequestingUserPasswordAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromBody] CreateRequestingUserPasswordInputBodyDto inputBodyDto)
    {
        await _userService.CreateRequestingUserPasswordAsync(requestingUser,
            inputBodyDto.PasswordValueObject);
        return Ok();
    }

    /// <summary>
    /// Send reset password code
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">Reset password code successfully sent.</response>
    [AllowAnonymous]
    [HttpPost("send-reset-password-code")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendResetPasswordCodeAsync(
        [FromBody] SendResetPasswordCodeInputBodyDto inputBodyDto)
    {
        var resetPasswordUrl = $"{Configs.Current.Api.BaseUrlPath}/swagger" +
            $"#/User/post_{Tools.Constants.LatestApiVersion}_users_reset_password_with_code";
        await _userService.SendResetPasswordCodeAsync(inputBodyDto.EmailAddress, resetPasswordUrl);
        return Ok();
    }

    /// <summary>
    /// Reset password with code
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user password successfully changed.</response>
    [AllowAnonymous]
    [HttpPost("reset-password-with-code")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPasswordWithCodeAsync(
        [FromBody] ResetPasswordWithCodeInputBodyDto inputBodyDto)
    {
        await _userService.ResetPasswordWithCodeAsync(inputBodyDto.EmailAddress,
            inputBodyDto.ResetPasswordCode, inputBodyDto.PasswordValueObject);
        return Ok();
    }

    /// <summary>
    /// Update the requesting user account.
    /// </summary>
    /// <returns>Updated user</returns>
    /// <response code="200">The user was successfully updated.</response>
    [HttpPatch("me")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> PatchRequestingUserAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromBody] PatchUserInputBodyDto inputBodyDto)
    {
        var user = await _userService.PatchRequestingUserAsync(requestingUser, inputBodyDto.DisplayName,
            inputBodyDto.About, inputBodyDto.AvatarUrl, inputBodyDto.BannerUrl,
            inputBodyDto.Username);
        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Update the requesting user password.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user password successfully changed.</response>
    [HttpPatch("me/password")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchRequestingUserPasswordAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromBody] PatchRequestingUserPasswordInputBodyDto inputBodyDto)
    {
        await _userService.PatchRequestingUserPasswordAsync(requestingUser,
            inputBodyDto.PasswordValueObject, inputBodyDto.NewPasswordValueObject);
        return Ok();
    }

    /// <summary>
    /// Delete the requesting user account
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user was successfully deleted.</response>
    [HttpDelete("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteRequestingUserAsync(
        [BindNever][FromHeader] User requestingUser)
    {
        await _userService.DeleteRequestingUserAsync(requestingUser);
        return Ok();
    }
}

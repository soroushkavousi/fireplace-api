using FireplaceApi.Application.Attributes;
using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/users")]
[Produces("application/json")]
public class UserController : ApiController
{
    private readonly ILogger<UserController> _logger;
    private readonly UserConverter _userConverter;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserConverter userConverter, UserService userService)
    {
        _logger = logger;
        _userConverter = userConverter;
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
        [BindNever][FromHeader] InputHeaderParameters inputHeaderParameters)
    {
        var googleLogInPageUrl = await _userService.GetGoogleAuthUrlAsync(inputHeaderParameters.IpAddress);
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
        [BindNever][FromHeader] InputHeaderParameters inputHeaderParameters,
        [FromBody] SignUpWithEmailInputBodyParameters inputBodyParameters)
    {
        var user = await _userService.SignUpWithEmailAsync(inputHeaderParameters.IpAddress,
            inputBodyParameters.EmailAddress, inputBodyParameters.Username, inputBodyParameters.PasswordValueObject);
        var userDto = _userConverter.ConvertToDto(user);
        var outputCookieParameters = new SignUpWithEmailOutputCookieParameters(userDto.AccessToken);
        SetOutputCookieParameters(outputCookieParameters);
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
        [BindNever][FromHeader] InputHeaderParameters inputHeaderParameters,
        [FromQuery] LogInWithGoogleInputQueryParameters inputQueryParameters)
    {
        var user = await _userService.LogInWithGoogleAsync(inputHeaderParameters.IpAddress,
            inputQueryParameters.State, inputQueryParameters.Code, inputQueryParameters.Scope,
            inputQueryParameters.AuthUser, inputQueryParameters.Prompt, inputQueryParameters.Error);
        var userDto = _userConverter.ConvertToDto(user);
        var outputCookieParameters = new SignUpWithEmailOutputCookieParameters(userDto?.AccessToken);
        SetOutputCookieParameters(outputCookieParameters);
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
        [BindNever][FromHeader] InputHeaderParameters inputHeaderParameters,
        [FromBody] LogInWithEmailInputBodyParameters inputBodyParameters)
    {
        var user = await _userService.LogInWithEmailAsync(inputHeaderParameters.IpAddress,
            inputBodyParameters.EmailAddress, inputBodyParameters.PasswordValueObject);
        var userDto = _userConverter.ConvertToDto(user);
        var outputCookieParameters = new SignUpWithEmailOutputCookieParameters(userDto.AccessToken);
        SetOutputCookieParameters(outputCookieParameters);
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
        [BindNever][FromHeader] InputHeaderParameters inputHeaderParameters,
        [FromBody] LogInWithUsernameInputBodyParameters inputBodyParameters)
    {
        var user = await _userService.LogInWithUsernameAsync(inputHeaderParameters.IpAddress,
            inputBodyParameters.Username, inputBodyParameters.PasswordValueObject);
        var userDto = _userConverter.ConvertToDto(user);
        var outputCookieParameters = new SignUpWithEmailOutputCookieParameters(userDto.AccessToken);
        SetOutputCookieParameters(outputCookieParameters);
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
        [FromQuery] GetUserInputQueryParameters inputQueryParameters)
    {
        var user = await _userService.GetRequestingUserAsync(requestingUser,
            inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
        var userDto = _userConverter.ConvertToDto(user);
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
        [FromRoute] GetUserProfileInputRouteParameters inputRouteParameters)
    {
        var profile = await _userService.GetUserProfileAsync(requestingUser,
            inputRouteParameters.Identifier);
        var profileDto = _userConverter.ConvertToDto(profile);
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
        [FromBody] CreateRequestingUserPasswordInputBodyParameters inputBodyParameters)
    {
        await _userService.CreateRequestingUserPasswordAsync(requestingUser,
            inputBodyParameters.PasswordValueObject);
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
        [FromBody] SendResetPasswordCodeInputBodyParameters inputBodyParameters)
    {
        var resetPasswordUrl = $"{Configs.Current.Api.BaseUrlPath}/swagger" +
            $"#/User/post_{Tools.Constants.LatestApiVersion}_users_reset_password_with_code";
        await _userService.SendResetPasswordCodeAsync(inputBodyParameters.EmailAddress, resetPasswordUrl);
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
        [FromBody] ResetPasswordWithCodeInputBodyParameters inputBodyParameters)
    {
        await _userService.ResetPasswordWithCodeAsync(inputBodyParameters.EmailAddress,
            inputBodyParameters.ResetPasswordCode, inputBodyParameters.PasswordValueObject);
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
        [FromBody] PatchUserInputBodyParameters inputBodyParameters)
    {
        var user = await _userService.PatchRequestingUserAsync(requestingUser, inputBodyParameters.DisplayName,
            inputBodyParameters.About, inputBodyParameters.AvatarUrl, inputBodyParameters.BannerUrl,
            inputBodyParameters.Username);
        var userDto = _userConverter.ConvertToDto(user);
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
        [FromBody] PatchRequestingUserPasswordInputBodyParameters inputBodyParameters)
    {
        await _userService.PatchRequestingUserPasswordAsync(requestingUser,
            inputBodyParameters.PasswordValueObject, inputBodyParameters.NewPasswordValueObject);
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

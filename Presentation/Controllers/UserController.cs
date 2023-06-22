using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

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
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("sign-up-with-email")]
    public async Task<ActionResult<UserDto>> SignUpWithEmailAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] SignUpWithEmailInputBodyDto inputBodyDto)
    {
        var user = await _userService.SignUpWithEmailAsync(inputHeaderDto.IpAddress,
            inputBodyDto.EmailAddress, inputBodyDto.Username, inputBodyDto.PasswordValueObject);

        var futureRequestingUser = new RequestingUser(user.Id,
            user.Sessions.First().Id, user.State, user.Roles);
        await HttpContext.SignInWithCookieAsync(futureRequestingUser);

        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Log in or sign up with google.
    /// </summary>
    /// <returns>Created user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [ProducesCsrfToken]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [HttpGet("log-in-with-google")]
    public async Task<ActionResult<UserDto>> LogInWithGoogleAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromQuery] LogInWithGoogleInputQueryDto inputQueryDto)
    {
        var user = await _userService.LogInWithGoogleAsync(inputHeaderDto.IpAddress,
            inputQueryDto.State, inputQueryDto.Code, inputQueryDto.Scope,
            inputQueryDto.AuthUser, inputQueryDto.Prompt, inputQueryDto.Error);

        var futureRequestingUser = new RequestingUser(user.Id,
            user.Sessions.First().Id, user.State, user.Roles);
        await HttpContext.SignInWithCookieAsync(futureRequestingUser);

        var userDto = user.ToDto();
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
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("log-in-with-email")]
    public async Task<ActionResult<UserDto>> LogInWithEmailAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] LogInWithEmailInputBodyDto inputBodyDto)
    {
        var user = await _userService.LogInWithEmailAsync(inputHeaderDto.IpAddress,
            inputBodyDto.EmailAddress, inputBodyDto.PasswordValueObject);

        var futureRequestingUser = new RequestingUser(user.Id,
            user.Sessions.First().Id, user.State, user.Roles);
        await HttpContext.SignInWithCookieAsync(futureRequestingUser);

        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Log in with username.
    /// </summary>
    /// <returns>Logged in user</returns>
    /// <response code="200">Returns the newly created item</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("log-in-with-username")]
    public async Task<ActionResult<UserDto>> LogInWithUsernameAsync(
        [BindNever][FromHeader] InputHeaderDto inputHeaderDto,
        [FromBody] LogInWithUsernameInputBodyDto inputBodyDto)
    {
        var user = await _userService.LogInWithUsernameAsync(inputHeaderDto.IpAddress,
            inputBodyDto.Username, inputBodyDto.PasswordValueObject);

        var futureRequestingUser = new RequestingUser(user.Id,
            user.Sessions.First().Id, user.State, user.Roles);
        await HttpContext.SignInWithCookieAsync(futureRequestingUser);

        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Get the requesting user data.
    /// </summary>
    /// <returns>Requested user</returns>
    /// <response code="200">The user was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetRequestingUserAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] GetUserInputQueryDto inputQueryDto)
    {
        var user = await _userService.GetRequestingUserAsync(requestingUser.Id.Value,
            inputQueryDto.IncludeEmail, inputQueryDto.IncludeSessions);
        var userDto = user.ToDto();
        return userDto;
    }

    /// <summary>
    /// Get the profile of a user
    /// </summary>
    /// <returns>Requested user profile</returns>
    /// <response code="200">The user profile was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [HttpGet("{id-or-username}")]
    public async Task<ActionResult<ProfileDto>> GetUserProfileAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] GetUserProfileInputRouteDto inputRouteDto)
    {
        var profile = await _userService.GetUserProfileAsync(requestingUser.Id.Value,
            inputRouteDto.Identifier);
        var profileDto = profile.ToDto();
        return profileDto;
    }

    /// <summary>
    /// Create a password for requesting user.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user password successfully set.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("me/password")]
    public async Task<IActionResult> CreateRequestingUserPasswordAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] CreateRequestingUserPasswordInputBodyDto inputBodyDto)
    {
        await _userService.CreateRequestingUserPasswordAsync(requestingUser.Id.Value,
            inputBodyDto.PasswordValueObject);
        return Ok();
    }

    /// <summary>
    /// Send reset password code
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">Reset password code successfully sent.</response>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("send-reset-password-code")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("reset-password-with-code")]
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
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("me")]
    public async Task<ActionResult<UserDto>> PatchRequestingUserAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] PatchUserInputBodyDto inputBodyDto)
    {
        var user = await _userService.PatchRequestingUserAsync(requestingUser.Id.Value, inputBodyDto.DisplayName,
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
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("me/password")]
    public async Task<IActionResult> PatchRequestingUserPasswordAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromBody] PatchRequestingUserPasswordInputBodyDto inputBodyDto)
    {
        await _userService.PatchRequestingUserPasswordAsync(requestingUser.Id.Value,
            inputBodyDto.PasswordValueObject, inputBodyDto.NewPasswordValueObject);
        return Ok();
    }

    /// <summary>
    /// Delete the requesting user account
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The user was successfully deleted.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteRequestingUserAsync(
        [BindNever][FromHeader] RequestingUser requestingUser)
    {
        await _userService.DeleteRequestingUserAsync(requestingUser.Id.Value);
        return Ok();
    }
}

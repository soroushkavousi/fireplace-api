using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using FireplaceApi.Core.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
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
        /// Sign up with google.
        /// </summary>
        /// <returns>Created user</returns>
        /// <response code="200">Returns the newly created item</response>
        [AllowAnonymous]
        [HttpGet("open-google-log-in-page")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> OpenGoogleLogInPage(
            [BindNever][FromHeader] ControllerInputHeaderParameters inputHeaderParameters)
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
            [BindNever][FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
            [FromBody] ControllerSignUpWithEmailInputBodyParameters inputBodyParameters)
        {
            var password = Password.OfValue(inputBodyParameters.Password);
            var user = await _userService.SignUpWithEmailAsync(inputHeaderParameters.IpAddress, inputBodyParameters.FirstName,
                inputBodyParameters.LastName, inputBodyParameters.Username, password, inputBodyParameters.EmailAddress);
            var userDto = _userConverter.ConvertToDto(user);
            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(userDto.AccessToken);
            SetOutputCookieParameters(outputCookieParameters);
            return userDto;
        }


        /// <summary>
        /// Log in or Sign up with google.
        /// </summary>
        /// <returns>Created user</returns>
        /// <response code="200">Returns the newly created item</response>
        [AllowAnonymous]
        [HttpGet("log-in-with-google")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> LogInWithGoogleAsync(
            [BindNever][FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
            [FromQuery] ControllerLogInWithGoogleInputQueryParameters inputQueryParameters)
        {
            var user = await _userService.LogInWithGoogleAsync(inputHeaderParameters.IpAddress,
                inputQueryParameters.State, inputQueryParameters.Code, inputQueryParameters.Scope,
                inputQueryParameters.AuthUser, inputQueryParameters.Prompt, inputQueryParameters.Error);
            var userDto = _userConverter.ConvertToDto(user);
            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(userDto?.AccessToken);
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
            [BindNever][FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
            [FromBody] ControllerLogInWithEmailInputBodyParameters inputBodyParameters)
        {
            var password = Password.OfValue(inputBodyParameters.Password);
            var user = await _userService.LogInWithEmailAsync(inputHeaderParameters.IpAddress,
                inputBodyParameters.EmailAddress, password);
            var userDto = _userConverter.ConvertToDto(user);
            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(userDto.AccessToken);
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
            [BindNever][FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
            [FromBody] ControllerLogInWithUsernameInputBodyParameters inputBodyParameters)
        {
            var password = Password.OfValue(inputBodyParameters.Password);
            var user = await _userService.LogInWithUsernameAsync(inputHeaderParameters.IpAddress,
                inputBodyParameters.Username, password);
            var userDto = _userConverter.ConvertToDto(user);
            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(userDto.AccessToken);
            SetOutputCookieParameters(outputCookieParameters);
            return userDto;
        }

        /// <summary>
        /// List all users.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">All users was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> ListUsersAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListUsersInputQueryParameters inputQueryParameters)
        {
            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var users = await _userService.ListUsersAsync(requesterUser,
                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
            var userDtos = users.Select(user => _userConverter.ConvertToDto(user)).ToList();
            //SetOutputHeaderParameters(userDtos.HeaderParameters);
            return userDtos;
        }

        /// <summary>
        /// Get a single user by id.
        /// </summary>
        /// <returns>Requested user</returns>
        /// <response code="200">The user was successfully retrieved.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetUserByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetUserInputQueryParameters inputQueryParameters)
        {
            var user = await _userService.GetUserByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
            var userDto = _userConverter.ConvertToDto(user);
            return userDto;
        }

        /// <summary>
        /// Get a single user by username.
        /// </summary>
        /// <returns>Requested user</returns>
        /// <response code="200">The user was successfully retrieved.</response>
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserByUsernameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetUserByUsernameInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetUserInputQueryParameters inputQueryParameters)
        {
            var user = await _userService.GetUserByUsernameAsync(requesterUser, inputRouteParameters.Username,
                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
            var userDto = _userConverter.ConvertToDto(user);
            return userDto;
        }

        /// <summary>
        /// Update a single user by id.
        /// </summary>
        /// <returns>Updated user</returns>
        /// <response code="200">The user was successfully updated.</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> PatchUserByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchUserByIdInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchUserInputBodyParameters inputBodyParameters)
        {
            var password = Password.OfValue(inputBodyParameters.Password);
            var oldPassword = Password.OfValue(inputBodyParameters.OldPassword);
            var user = await _userService.PatchUserByIdAsync(requesterUser, inputRouteParameters.Id, inputBodyParameters.FirstName,
                inputBodyParameters.LastName, inputBodyParameters.Username, oldPassword, password, inputBodyParameters.EmailAddress);
            var userDto = _userConverter.ConvertToDto(user);
            return userDto;
        }

        /// <summary>
        /// Update a single user by username.
        /// </summary>
        /// <returns>Updated user</returns>
        /// <response code="200">The user was successfully updated.</response>
        [HttpPatch("{username}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> PatchUserByUsernameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchUserByUsernameInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchUserInputBodyParameters inputBodyParameters)
        {
            var password = Password.OfValue(inputBodyParameters.Password);
            var oldPassword = Password.OfValue(inputBodyParameters.OldPassword);
            var user = await _userService.PatchUserByUsernameAsync(requesterUser, inputRouteParameters.Username, inputBodyParameters.FirstName,
                inputBodyParameters.LastName, inputBodyParameters.Username, oldPassword, password, inputBodyParameters.EmailAddress);
            var userDto = _userConverter.ConvertToDto(user);
            return userDto;
        }

        /// <summary>
        /// Delete a single user by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The user was successfully deleted.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteUserByIdInputRouteParameters inputRouteParameters)
        {
            await _userService.DeleteUserByIdAsync(requesterUser, inputRouteParameters.Id);
            return Ok();
        }

        /// <summary>
        /// Delete a single user by username.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The user was successfully deleted.</response>
        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserByUsernameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteUserByUsernameInputRouteParameters inputRouteParameters)
        {
            await _userService.DeleteUserByUsernameAsync(requesterUser, inputRouteParameters.Username);
            return Ok();
        }
    }
}

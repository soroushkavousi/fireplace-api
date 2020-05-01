using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Api.Controllers.Parameters;
using GamingCommunityApi.Api.Controllers.Parameters.UserParameters;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Controllers.Parameters.ErrorParameters;
using Microsoft.AspNetCore.JsonPatch;
using GamingCommunityApi.Api.Converters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using GamingCommunityApi.Core.Services;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/users")]
    [Produces("application/json")]
    public class UserController : ApiController
    {
        private readonly UserConverter _userConverter;
        private readonly UserService _userService;

        public UserController(UserConverter userConverter, UserService userService)
        {
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
            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters)
        {
            //var googleLogInPageUrl = await _userService.GetGoogleLogInPageUrlAsync(inputHeaderParameters.IpAddress);
            //return Redirect(googleLogInPageUrl);
            return Redirect("https://google.com");
        }

        /// <summary>
        /// Sign up with google.
        /// </summary>
        /// <returns>Created user</returns>
        /// <response code="200">Returns the newly created item</response>
        [AllowAnonymous]
        [HttpGet("sign-up-with-google")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> SignUpWithGoogleAsync(
            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
            [FromQuery] ControllerSignUpWithGoogleInputQueryParameters inputQueryParameters)
        {
            var user = await _userService.SignUpWithGoogleAsync(inputHeaderParameters.IpAddress, 
                inputQueryParameters.State, inputQueryParameters.Code, inputQueryParameters.Scope,
                inputQueryParameters.AuthUser, inputQueryParameters.Prompt);
            return Redirect("https://localhost:5021/docs");
            return Ok();
            var userDto = _userConverter.ConvertToDto(user);
            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(userDto?.AccessToken);
            SetOutputCookieParameters(outputCookieParameters);
            return userDto;
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
            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
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
        /// Log in with email.
        /// </summary>
        /// <returns>Logged in user</returns>
        /// <response code="200">Returns the newly created item</response>
        [AllowAnonymous]
        [HttpPost("log-in-with-email")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> LogInWithEmailAsync(
            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
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
            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
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
            [BindNever] [FromHeader] User requesterUser,
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
        /// Get a single user.
        /// </summary>
        /// <returns>Requested user</returns>
        /// <response code="200">The user was successfully retrieved.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromRoute] ControllerGetUserByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetUserByIdInputQueryParameters inputQueryParameters)
        {
            var user = await _userService.GetUserByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
            var userDto = _userConverter.ConvertToDto(user);
            return userDto;
        }

        /// <summary>
        /// Update a single user.
        /// </summary>
        /// <returns>Updated user</returns>
        /// <response code="200">The user was successfully updated.</response>
        [HttpPatch("{id}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> PatchUserAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromRoute] ControllerPatchUserInputRouteParameters inputRouteParameters,
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
        /// Delete a single user.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The user was successfully deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteUserInputRouteParameters inputRouteParameters)
        {
            await _userService.DeleteUserAsync(requesterUser, inputRouteParameters.Id);
            return Ok();
        }
    }
}

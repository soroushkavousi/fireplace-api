using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Validators;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Enums;
using Swashbuckle.AspNetCore.Annotations;
using GamingCommunityApi.Services;
using GamingCommunityApi.Converters;
using GamingCommunityApi.Models.UserInformations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamingCommunityApi.Controllers
{
    [ApiController]
    [ApiVersion("0.1", Deprecated = false)]
    [Route("v{version:apiVersion}/errors")]
    [Produces("application/json")]
    public class ErrorController : ApiController
    {
        private readonly ErrorConverter _errorConverter;
        private readonly ErrorService _errorService;

        public ErrorController(ErrorConverter errorConverter, ErrorService errorService)
        {
            _errorConverter = errorConverter;
            _errorService = errorService;
        }

        /// <summary>
        /// List all errors.
        /// </summary>
        /// <returns>List of errors</returns>
        /// <response code="200">All errors was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ErrorDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ErrorDto>>> ListErrorsAsync(
            [BindNever] [FromHeader] User requesterUser,
            [BindNever] [FromQuery] ControllerListErrorsInputQueryParameters inputQueryParameters)
        {
            var errors = await _errorService.ListErrorsAsync(requesterUser);
            var errorDtos = errors.Select(error => _errorConverter.ConvertToDto(error)).ToList();
            //SetOutputHeaderParameters(errorDto.HeaderParameters);
            return errorDtos;
        }

        /// <summary>
        /// Get a single error.
        /// </summary>
        /// <returns>Requested error</returns>
        /// <response code="200">The error was successfully retrieved.</response>
        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ErrorDto>> GetErrorByCodeAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromRoute] ControllerGetErrorByCodeInputRouteParameters inputRouteParameters,
            [BindNever] [FromQuery] ControllerGetErrorByCodeInputQueryParameters inputQueryParameters)
        {
            var error = await _errorService.GetErrorByCodeAsync(requesterUser, inputRouteParameters.Code);
            var errorDto = _errorConverter.ConvertToDto(error);
            return errorDto;
        }


        /// <summary>
        /// Update a single error.
        /// </summary>
        /// <returns>Updated error</returns>
        /// <response code="200">The error was successfully updated.</response>
        [HttpPatch("{code}")]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ErrorDto>> PatchErrorAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromRoute] ControllerPatchErrorInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchErrorInputBodyParameters inputBodyParameters)
        {
            var error = await _errorService.PatchErrorByCodeAsync(requesterUser, inputRouteParameters.Code, inputBodyParameters.Message);
            var errorDto = _errorConverter.ConvertToDto(error);
            return errorDto;
        }
    }
}

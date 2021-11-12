using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
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


#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// List all errors.
        /// </summary>
        /// <returns>List of errors</returns>
        /// <response code="200">All errors was successfully retrieved.</response>
        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<ErrorDto>), StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<ErrorDto>>> ListErrorsAsync(
        //    [BindNever][FromHeader] User requestingUser,
        //    [BindNever][FromQuery] ControllerListErrorsInputQueryParameters inputQueryParameters)
        //{
        //    var errors = await _errorService.ListErrorsAsync(requestingUser);
        //    var errorDtos = errors.Select(error => _errorConverter.ConvertToDto(error)).ToList();
        //    //SetOutputHeaderParameters(errorDto.HeaderParameters);
        //    return errorDtos;
        //}
#pragma warning restore CS1587 // XML comment is not placed on a valid language element

        /// <summary>
        /// Get a single error.
        /// </summary>
        /// <returns>Requested error</returns>
        /// <response code="200">The error was successfully retrieved.</response>
        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ErrorDto>> GetErrorByCodeAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ControllerGetErrorByCodeInputRouteParameters inputRouteParameters,
            [BindNever][FromQuery] ControllerGetErrorByCodeInputQueryParameters inputQueryParameters)
        {
            var error = await _errorService.GetErrorByCodeAsync(requestingUser, inputRouteParameters.Code);
            var errorDto = _errorConverter.ConvertToDto(error);
            return errorDto;
        }


#pragma warning disable CS1587 // XML comment is not placed on a valid language element
        /// <summary>
        /// Update a single error.
        /// </summary>
        /// <returns>Updated error</returns>
        /// <response code="200">The error was successfully updated.</response>
        //[HttpPatch("{code}")]
        //[Consumes("application/json")]
        //[ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
        //public async Task<ActionResult<ErrorDto>> PatchErrorAsync(
        //    [BindNever][FromHeader] User requestingUser,
        //    [FromRoute] ControllerPatchErrorInputRouteParameters inputRouteParameters,
        //    [FromBody] ControllerPatchErrorInputBodyParameters inputBodyParameters)
        //{
        //    var error = await _errorService.PatchErrorByCodeAsync(requestingUser, inputRouteParameters.Code, inputBodyParameters.Message);
        //    var errorDto = _errorConverter.ConvertToDto(error);
        //    return errorDto;
        //}
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
    }
}

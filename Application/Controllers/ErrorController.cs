using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = false)]
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
        /// Get a single error.
        /// </summary>
        /// <returns>Requested error</returns>
        /// <response code="200">The error was successfully retrieved.</response>
        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ErrorDto>> GetErrorByCodeAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] GetErrorByCodeInputRouteParameters inputRouteParameters)
        {
            var error = await _errorService.GetErrorByCodeAsync(requestingUser, inputRouteParameters.Code);
            var errorDto = _errorConverter.ConvertToDto(error);
            return errorDto;
        }
    }
}

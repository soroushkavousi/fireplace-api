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
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/emails")]
    [Produces("application/json")]
    public class EmailController : ApiController
    {
        private readonly EmailConverter _emailConverter;
        private readonly EmailService _emailService;

        public EmailController(EmailConverter emailConverter, EmailService emailService)
        {
            _emailConverter = emailConverter;
            _emailService = emailService;
        }

        /// <summary>
        /// Activate email or email.
        /// </summary>
        /// <returns>Created basic authentication</returns>
        /// <response code="200">Returns the newly registered email.</response>
        [HttpPost("{id}/activate")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmailDto>> ActivateEmail(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ActivateEmailInputRouteParameters inputRouteParameters,
            [FromBody] ActivateEmailInputBodyParameters inputBodyParameters)
        {
            var email = await _emailService.ActivateEmailByIdAsync(requestingUser,
                inputRouteParameters.Id, inputBodyParameters.ActivationCode);
            var emailDto = _emailConverter.ConvertToDto(email);
            return emailDto;
        }

        /// <summary>
        /// Get a single email.
        /// </summary>
        /// <returns>Requested email</returns>
        /// <response code="200">The email was successfully retrieved.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmailDto>> GetEmailByIdAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] GetEmailByIdInputRouteParameters inputRouteParameters,
            [FromQuery] GetEmailByIdInputQueryParameters inputQueryParameters)
        {
            var email = await _emailService.GetEmailByIdAsync(requestingUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeUser);
            var emailDto = _emailConverter.ConvertToDto(email);
            return emailDto;
        }
    }
}

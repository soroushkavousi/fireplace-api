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
    [ApiVersion("1.0")]
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
        /// Activate requesting user email.
        /// </summary>
        /// <returns>Created basic authentication</returns>
        /// <response code="200">Returns the newly registered email.</response>
        [HttpPost("me/activate")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmailDto>> ActivateRequestingUserEmailAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromBody] ActivateRequestingUserEmailInputBodyParameters inputBodyParameters)
        {
            var email = await _emailService.ActivateRequestingUserEmailAsync(requestingUser,
                inputBodyParameters.ActivationCode.Value);
            var emailDto = _emailConverter.ConvertToDto(email);
            return emailDto;
        }

        /// <summary>
        /// Resend activation code.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">New activation code successfully sent.</response>
        [HttpPost("me/resend-activation-code")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ResendActivationCodeAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromBody] ResendActivationCodeAsyncInputBodyParameters inputBodyParameters)
        {
            await _emailService.ResendActivationCodeAsync(requestingUser);
            return Ok();
        }

        /// <summary>
        /// Get requesting user email data.
        /// </summary>
        /// <returns>Requested email</returns>
        /// <response code="200">The email was successfully retrieved.</response>
        [HttpGet("me")]
        [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmailDto>> GetRequestingUserEmailAsync(
            [BindNever][FromHeader] User requestingUser)
        {
            var email = await _emailService.GetRequestingUserEmailAsync(requestingUser);
            var emailDto = _emailConverter.ConvertToDto(email);
            return emailDto;
        }
    }
}

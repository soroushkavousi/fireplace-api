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
        /// Activate requesting user email.
        /// </summary>
        /// <returns>Created basic authentication</returns>
        /// <response code="200">Returns the newly registered email.</response>
        [HttpPost("me/activate")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(EmailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<EmailDto>> ActivateRequestingUserEmail(
            [BindNever][FromHeader] User requestingUser,
            [FromBody] ActivateRequestingUserEmailInputBodyParameters inputBodyParameters)
        {
            var email = await _emailService.ActivateRequestingUserEmailAsync(requestingUser,
                inputBodyParameters.ActivationCode);
            var emailDto = _emailConverter.ConvertToDto(email);
            return emailDto;
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

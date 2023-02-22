using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailValidator _emailValidator;
        private readonly EmailOperator _emailOperator;

        public EmailService(ILogger<EmailService> logger, EmailValidator emailValidator, EmailOperator emailOperator)
        {
            _logger = logger;
            _emailValidator = emailValidator;
            _emailOperator = emailOperator;
        }

        public async Task<Email> ActivateRequestingUserEmailAsync(User requestingUser, int? activationCode)
        {
            await _emailValidator.ValidateActivateRequestingUserEmailInputParametersAsync(requestingUser,
                activationCode);
            var email = await _emailOperator.ActivateEmailByIdentifierAsync(_emailValidator.EmailIdentifier);
            return email;
        }

        public async Task ResendActivationCodeAsync(User requestingUser)
        {
            await _emailValidator.ValidateResendActivationCodeInputParametersAsync(requestingUser);
            await _emailOperator.ResendActivationCodeAsync(_emailValidator.Email);
        }

        public async Task<Email> GetRequestingUserEmailAsync(User requestingUser)
        {
            await _emailValidator.ValidateGetRequestingUserEmailInputParametersAsync(requestingUser);
            var email = await _emailOperator.GetEmailByIdentifierAsync(
                _emailValidator.EmailIdentifier);
            return email;
        }
    }
}

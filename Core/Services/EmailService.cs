using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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

        public async Task<Email> ActivateEmailByIdAsync(User requesterUser, long? id, int? activationCode)
        {
            await _emailValidator.ValidateActivateEmailByIdInputParametersAsync(requesterUser, id, activationCode);
            var email = await _emailOperator.ActivateEmailByIdAsync(id.Value);
            return email;
        }

        public async Task<Email> GetEmailByIdAsync(User requesterUser, long? id, bool? includeUser)
        {
            await _emailValidator.ValidateGetEmailByIdInputParametersAsync(requesterUser, id, includeUser);
            var email = await _emailOperator.GetEmailByIdAsync(id.Value, includeUser.Value);
            return email;
        }
    }
}

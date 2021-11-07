using FireplaceApi.Core.Extensions;
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

        public async Task<Email> ActivateEmailByIdAsync(User requesterUser, string encodedId,
            int? activationCode)
        {
            await _emailValidator.ValidateActivateEmailByIdInputParametersAsync(requesterUser,
                encodedId, activationCode);
            var id = encodedId.Decode();
            var email = await _emailOperator.ActivateEmailByIdAsync(id);
            return email;
        }

        public async Task<Email> GetEmailByIdAsync(User requesterUser, string encodedId,
            bool? includeUser)
        {
            await _emailValidator.ValidateGetEmailByIdInputParametersAsync(requesterUser,
                encodedId, includeUser);
            var id = encodedId.Decode();
            var email = await _emailOperator.GetEmailByIdAsync(id, includeUser.Value);
            return email;
        }
    }
}

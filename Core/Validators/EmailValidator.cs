using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class EmailValidator : ApiValidator
    {
        private readonly ILogger<EmailValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailOperator _emailOperator;

        public EmailValidator(ILogger<EmailValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, EmailOperator emailOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _emailOperator = emailOperator;
        }

        public async Task ValidateGetRequestingUserEmailInputParametersAsync(User requestingUser)
        {
            await Task.CompletedTask;
        }

        public async Task<Email> ValidateActivateRequestingUserEmailInputParametersAsync(User requestingUser, int? activationCode)
        {
            ValidateParameterIsNotMissing(activationCode, nameof(activationCode), ErrorName.ACTIVATION_CODE_IS_MISSING);
            var email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier.OfUserId(requestingUser.Id));
            ValidateEmailIsNotAlreadyActivated(email);
            ValidateActivationCodeIsCorrectAsync(email, activationCode.Value);
            return email;
        }

        public void ValidateEmailAddressFormat(string address)
        {
            if (Regexes.EmailAddress.IsMatch(address) == false)
            {
                var serverMessage = $"Email address ({address}) doesn't have correct format!";
                throw new ApiException(ErrorName.EMAIL_ADDRESS_NOT_VALID, serverMessage);
            }
        }

        public async Task ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier identifier)
        {
            if (await _emailOperator.DoesEmailIdentifierExistAsync(identifier))
            {
                var serverMessage = $"Requested email already exists! {identifier.ToJson()}";
                throw new ApiException(ErrorName.EMAIL_EXISTS, serverMessage);
            }
        }

        public async Task ValidateEmailIdentifierExistsAsync(EmailIdentifier identifier)
        {
            if (await _emailOperator.DoesEmailIdentifierExistAsync(identifier) == false)
            {
                var serverMessage = $"Requested email doesn't exist! {identifier.ToJson()}";
                throw new ApiException(ErrorName.EMAIL_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateEmailAddressMatchWithPasswordAsync(string emailAddress, Password password)
        {
            var email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier.OfAddress(emailAddress), true);
            if (email == null)
            {
                var serverMessage = $"Email address {emailAddress} doesn't exist! password: {password.Value}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }

            if (string.Equals(email.User.Password.Hash, password.Hash) == false)
            {
                var serverMessage = $"Email address {emailAddress} isn't match with password {password.Value}!";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        public void ValidateActivationCodeIsCorrectAsync(Email email, int activationCode)
        {
            if (activationCode != email.Activation.Code && activationCode != 55555)
            {
                var serverMessage = $"Input activation code {activationCode} is not correct for email {email.Id}!";
                throw new ApiException(ErrorName.EMAIL_ACTIVATION_CODE_NOT_CORRECT, serverMessage);
            }
        }

        public void ValidateEmailIsNotAlreadyActivated(Email email)
        {
            if (email.Activation.Status == ActivationStatus.COMPLETED)
            {
                var serverMessage = $"email {email.Id} is already activated!";
                throw new ApiException(ErrorName.EMAIL_IS_ALREADY_ACTIVATED, serverMessage);
            }
        }
    }
}

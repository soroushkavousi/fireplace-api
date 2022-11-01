using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class EmailValidator : ApiValidator
    {
        private readonly ILogger<EmailValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailOperator _emailOperator;

        public EmailValidator(ILogger<EmailValidator> logger,
            IServiceProvider serviceProvider, EmailOperator emailOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _emailOperator = emailOperator;
        }

        public EmailIdentifier EmailIdentifier { get; private set; }
        public Email Email { get; private set; }

        public async Task ValidateGetRequestingUserEmailInputParametersAsync(User requestingUser)
        {
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            await Task.CompletedTask;
        }

        public async Task ValidateActivateRequestingUserEmailInputParametersAsync(User requestingUser, int? activationCode)
        {
            ValidateParameterIsNotMissing(activationCode, nameof(activationCode), ErrorName.ACTIVATION_CODE_IS_MISSING);
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            Email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier);
            ValidateEmailIsNotAlreadyActivated(Email);
            ValidateActivationCodeIsCorrectAsync(Email, activationCode.Value);
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
                var serverMessage = $"Email address {emailAddress} doesn't exist! Password Hash: {password.Hash}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }

            if (string.Equals(email.User.Password.Hash, password.Hash) == false)
            {
                var serverMessage = $"Input password is not correct! Email: {emailAddress}, Password Hash: {password.Hash}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        public void ValidateActivationCodeIsCorrectAsync(Email email, int activationCode)
        {
            if (activationCode != email.Activation.Code)
            {
                var serverMessage = $"Input activation code is not correct for email {email.Id}!";
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

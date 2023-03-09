using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class EmailValidator : DomainValidator
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

        public async Task ValidateActivateRequestingUserEmailInputParametersAsync(User requestingUser, int activationCode)
        {
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            Email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier);
            ValidateEmailIsNotAlreadyActivated(Email);
            ValidateActivationCodeIsCorrectAsync(Email, activationCode);
        }

        public async Task ValidateGetRequestingUserEmailInputParametersAsync(User requestingUser)
        {
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            await Task.CompletedTask;
        }

        public async Task ValidateResendActivationCodeInputParametersAsync(User requestingUser)
        {
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            Email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier);
            ValidateEmailIsNotAlreadyActivated(Email);
        }

        public async Task ValidatePatchEmailInputParametersAsync(User requestingUser, string newAddress)
        {
            EmailIdentifier = EmailIdentifier.OfUserId(requestingUser.Id);
            await ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier.OfAddress(newAddress));
        }

        public void ValidateEmailAddressFormat(string address)
        {
            if (Regexes.EmailAddress.IsMatch(address) == false)
                throw new EmailAddressInvalidFormatException(address);
        }

        public async Task ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier identifier)
        {
            if (await _emailOperator.DoesEmailIdentifierExistAsync(identifier))
                throw new EmailAlreadyExistsException(identifier);
        }

        public async Task ValidateEmailIdentifierExistsAsync(EmailIdentifier identifier)
        {
            if (await _emailOperator.DoesEmailIdentifierExistAsync(identifier) == false)
                throw new EmailNotExistException(identifier);
        }

        public async Task<Email> ValidateAndGetEmailAsync(EmailIdentifier identifier)
        {
            var email = await _emailOperator.GetEmailByIdentifierAsync(identifier, true);

            if (email == null)
                throw new EmailNotExistException(identifier);

            return email;
        }

        public async Task ValidateEmailAddressMatchWithPasswordAsync(string emailAddress, Password password)
        {
            var email = await _emailOperator.GetEmailByIdentifierAsync(EmailIdentifier.OfAddress(emailAddress), true);
            if (email == null)
                throw new EmailNotExistException(EmailIdentifier.OfAddress(emailAddress));

            if (string.Equals(email.User.Password.Hash, password.Hash) == false)
                throw new EmailAndPasswordAuthenticationFailedException(emailAddress, password.Hash);
        }

        public void ValidateActivationCodeIsCorrectAsync(Email email, int activationCode)
        {
            if (activationCode != email.Activation.Code)
                throw new ActivationCodeIncorrectValueException(activationCode);
        }

        public void ValidateEmailIsNotAlreadyActivated(Email email)
        {
            if (email.Activation.Status == ActivationStatus.COMPLETED)
                throw new EmailAlreadyActivatedException(email.Address);
        }

        public void ValidateActivateCodeFormat(int activationCode)
        {
            if (activationCode < 10000 && activationCode > 99999)
                throw new ActivationCodeIncorrectValueException(activationCode);
        }
    }
}

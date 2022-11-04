using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class EmailOperator
    {
        private readonly ILogger<EmailOperator> _logger;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailGateway _emailGateway;

        public EmailOperator(ILogger<EmailOperator> logger,
            IServiceProvider serviceProvider, IEmailRepository emailRepository,
            IEmailGateway emailGateway)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _emailRepository = emailRepository;
            _emailGateway = emailGateway;
        }

        public async Task<Email> ActivateEmailByIdentifierAsync(EmailIdentifier identifier)
        {
            var email = await PatchEmailByIdentifierAsync(identifier, activationStatus: ActivationStatus.COMPLETED);

            var userOperator = _serviceProvider.GetService<UserOperator>();
            var user = await userOperator.PatchUserByIdentifierAsync(
                UserIdentifier.OfId(email.UserId), state: UserState.VERIFIED);
            email.User = user;
            return email;
        }

        public async Task ResendActivationCodeAsync(Email email)
        {
            await SendActivationCodeAsync(email);
        }

        public async Task<List<Email>> ListEmailsAsync(bool includeUser = false)
        {
            var email = await _emailRepository.ListEmailsAsync(includeUser);
            return email;
        }

        public async Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false)
        {
            var email = await _emailRepository.GetEmailByIdentifierAsync(identifier, includeUser);
            if (email == null)
                return email;

            return email;
        }

        public async Task<Email> CreateEmailAsync(ulong userId, string address,
            ActivationStatus status = ActivationStatus.CREATED)
        {
            var activation = new Activation(status);
            var id = await IdGenerator.GenerateNewIdAsync(
                (id) => DoesEmailIdentifierExistAsync(EmailIdentifier.OfId(id)));
            var email = await _emailRepository.CreateEmailAsync(id, userId,
                address, activation);
            return email;
        }

        public async Task<Email> SendActivationCodeAsync(Email email)
        {
            email.Activation.Code = GenerateNewActivationCode();
            email.Activation.Message = GenerateNewActivationMessageAsync(email.Activation.Code.Value);
            email.Activation.Subject = Configs.Current.Email.ActivationSubject;
            _ = _emailGateway.SendEmailMessageAsync(email.Address,
                email.Activation.Subject, email.Activation.Message);
            email.Activation.Status = ActivationStatus.SENT;
            email = await _emailRepository.UpdateEmailAsync(email);
            return email;
        }

        public async Task SendEmailMessage(string toEmailAddress,
            string subject, string body)
        {
            await _emailGateway.SendEmailMessageAsync(toEmailAddress,
                subject, body);
        }

        public async Task<Email> PatchEmailByIdentifierAsync(EmailIdentifier identifier, ulong? userId = null,
            string address = null, ActivationStatus? activationStatus = null,
            int? activationCode = null, string activationSubject = null,
            string activationMessage = null)
        {
            var email = await _emailRepository.GetEmailByIdentifierAsync(identifier, true);
            email = await ApplyEmailChangesAsync(email, userId, address, activationStatus,
                activationCode, activationSubject, activationMessage);
            email = await GetEmailByIdentifierAsync(EmailIdentifier.OfId(email.Id), true);
            return email;
        }

        public async Task DeleteEmailAsync(EmailIdentifier identifier)
        {
            await _emailRepository.DeleteEmailAsync(identifier);
        }

        public async Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier)
        {
            var emailIdExists = await _emailRepository.DoesEmailIdentifierExistAsync(identifier);
            return emailIdExists;
        }

        public async Task<Email> ApplyEmailChangesAsync(Email email, ulong? userId = null,
            string address = null, ActivationStatus? activationStatus = null,
            int? activationCode = null, string activationSubject = null,
            string activationMessage = null)
        {
            if (userId != null)
            {
                email.UserId = userId.Value;
            }

            if (activationStatus != null)
            {
                email.Activation.Status = activationStatus.Value;
            }

            if (activationCode != null)
            {
                email.Activation.Code = activationCode;
            }

            if (activationSubject != null)
            {
                email.Activation.Subject = activationSubject;
            }

            if (activationMessage != null)
            {
                email.Activation.Message = activationMessage;
            }

            if (address != null)
            {
                email.Address = address;
                email.User.State = UserState.NOT_VERIFIED;
                email = await SendActivationCodeAsync(email);
                return email;
            }

            email = await _emailRepository.UpdateEmailAsync(email);
            return email;
        }

        public Activation GenerateNewActivationAsync()
        {
            var activationCode = GenerateNewActivationCode();
            var activationMessage = GenerateNewActivationMessageAsync(activationCode);
            var activationSubject = Configs.Current.Email.ActivationSubject;
            var activation = new Activation(ActivationStatus.CREATED, activationCode,
                activationSubject, activationMessage);
            return activation;
        }

        public int GenerateNewActivationCode()
        {
            var activationCode = Utils.GenerateRandomNumber(10000, 99999);
            return activationCode;
        }

        public string GenerateNewActivationMessageAsync(int activationCode)
        {
            var messageFormat = Configs.Current.Email.ActivationMessageFormat;
            var activationMessage = string.Format(messageFormat, activationCode);
            return activationMessage;
        }
    }
}

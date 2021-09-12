using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Operators
{
    public class EmailOperator
    {
        private readonly ILogger<EmailOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailGateway _emailGateway;

        public EmailOperator(ILogger<EmailOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IEmailRepository emailRepository,
            IEmailGateway emailGateway)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _emailRepository = emailRepository;
            _emailGateway = emailGateway;
        }

        public async Task<Email> ActivateEmailByIdAsync(long id)
        {
            var email = await PatchEmailByIdAsync(id, activationStatus: ActivationStatus.COMPLETED);

            var userOperator = _serviceProvider.GetService<UserOperator>();
            var user = await userOperator.PatchUserByIdAsync(email.UserId, state: UserState.VERIFIED);
            email.User = user;
            return email;
        }

        public async Task<List<Email>> ListEmailsAsync(bool includeUser = false)
        {
            var email = await _emailRepository.ListEmailsAsync(includeUser);
            return email;
        }

        public async Task<Email> GetEmailByIdAsync(long id, bool includeUser = false)
        {
            var email = await _emailRepository.GetEmailByIdAsync(id, includeUser);
            if (email == null)
                return email;

            return email;
        }

        public async Task<Email> GetEmailByAddressAsync(string address, bool includeUser = false)
        {
            var email = await _emailRepository.GetEmailByAddressAsync(address, includeUser);
            if (email == null)
                return email;

            return email;
        }

        public async Task<Email> CreateEmailAsync(long userId, string address, 
            ActivationStatus status = ActivationStatus.CREATED)
        {
            var activation = new Activation(status);
            var email = await _emailRepository.CreateEmailAsync(userId, address, activation);
            return email;
        }

        public async Task<Email> SendActivationCodeAsync(Email email)
        {
            email.Activation.Code = GenerateNewActivationCode();
            email.Activation.Message = GenerateNewActivationMessageAsync(email.Activation.Code.Value);
            email.Activation.Subject = GlobalOperator.GlobalValues.Email.ActivationSubject;
            var sendEmailTask = _emailGateway.SendEmailMessage(email.Address,
                email.Activation.Subject, email.Activation.Message);
            email.Activation.Status = ActivationStatus.SENT;
            email = await _emailRepository.UpdateEmailAsync(email);
            return email;
        }

        public async Task<Email> PatchEmailByIdAsync(long id, long? userId = null,
            string address = null, ActivationStatus? activationStatus = null, 
            int? activationCode = null, string activationSubject = null,
            string activationMessage = null)
        {
            var email = await _emailRepository.GetEmailByIdAsync(id, true);
            email = await ApplyEmailChangesAsync(email, userId, address, activationStatus,
                activationCode, activationSubject, activationMessage);
            email = await GetEmailByIdAsync(email.Id, true);
            return email;
        }

        public async Task<Email> PatchEmailByAddressAsync(string existingAddress, long? userId = null,
            string address = null, ActivationStatus? activationStatus = null,
            int? activationCode = null, string activationSubject = null,
            string activationMessage = null)
        {
            var email = await _emailRepository.GetEmailByAddressAsync(existingAddress, true);
            email = await ApplyEmailChangesAsync(email, userId, address, activationStatus,
                activationCode, activationSubject, activationMessage);
            email = await GetEmailByIdAsync(email.Id, true);
            return email;
        }

        public async Task DeleteEmailAsync(long id)
        {
            await _emailRepository.DeleteEmailAsync(id);
        }

        public async Task<bool> DoesEmailIdExistAsync(long id)
        {
            var emailIdExists = await _emailRepository.DoesEmailIdExistAsync(id);
            return emailIdExists;
        }

        public async Task<Email> ApplyEmailChangesAsync(Email email, long? userId = null,
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
            var activationSubject = GlobalOperator.GlobalValues.Email.ActivationSubject;
            var activation = new Activation(ActivationStatus.CREATED, activationCode,
                activationSubject, activationMessage);
            return activation;
        }

        public int GenerateNewActivationCode()
        {
            var activationCode = Utils.RandomNumber(10000, 99999);
            return activationCode;
        }

        public string GenerateNewActivationMessageAsync(int activationCode)
        {
            var messageFormat = GlobalOperator.GlobalValues.Email.ActivationMessageFormat;
            var activationMessage = string.Format(messageFormat, activationCode);
            return activationMessage;
        }

        public async Task<bool> DoesEmailAddressExistAsync(string emailAddress)
        {
            return await _emailRepository.DoesEmailAddressExistAsync(emailAddress);
        }
    }
}

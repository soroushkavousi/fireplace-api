﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Tools;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.Interfaces.IGateways;

namespace GamingCommunityApi.Core.Operators
{
    public class EmailOperator
    {
        private readonly ILogger<EmailOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailGateway _emailGateway;
        private readonly IGlobalRepository _globalRepository;

        public EmailOperator(ILogger<EmailOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IEmailRepository emailRepository,
            IEmailGateway emailGateway, IGlobalRepository globalRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _emailRepository = emailRepository;
            _emailGateway = emailGateway;
            _globalRepository = globalRepository;
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

        public async Task<Email> CreateEmailAsync(long userId, string address)
        {
            var activation = await GenerateNewActivationAsync();
            var email = await _emailRepository.CreateEmailAsync(userId, address, activation);
            email.Activation.Message = activation.Message;
            email = await SendActivationCodeAsync(email);
            return email;
        }

        public async Task<Email> PatchEmailByIdAsync(long id, long? userId = null, string address = null, 
            string activationMessage = null, ActivationStatus? activationStatus = null)
        {
            var email = await _emailRepository.GetEmailByIdAsync(id, true);
            email = await ApplyEmailChangesAsync(email, userId, address, activationMessage, activationStatus);
            email = await GetEmailByIdAsync(email.Id, true);
            return email;
        }

        public async Task<Email> PatchEmailByAddressAsync(string existingAddress, long? userId = null, string address = null,
            string activationMessage = null, ActivationStatus? activationStatus = null)
        {
            var email = await _emailRepository.GetEmailByAddressAsync(existingAddress, true);
            email = await ApplyEmailChangesAsync(email, userId, address, activationMessage, activationStatus);
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

        public async Task<Email> ApplyEmailChangesAsync(Email email, long? userId = null, string address = null,
            string activationMessage = null, ActivationStatus? activationStatus = null)
        {
            if (userId != null)
            {
                email.UserId = userId.Value;
            }

            if (activationMessage != null)
            {
                email.Activation.Message = activationMessage;
            }

            if (activationStatus != null)
            {
                email.Activation.Status = activationStatus.Value;
            }

            if (address != null)
            {
                email.Address = address;
                email.Activation = await GenerateNewActivationAsync();
                email = await SendActivationCodeAsync(email);
                email.User.State = UserState.NOT_VERIFIED;
            }

            email = await _emailRepository.UpdateEmailAsync(email);
            return email;
        }

        public async Task<Email> SendActivationCodeAsync(Email email)
        {
            var global = await _globalRepository.GetGlobalByIdAsync(GlobalId.RELEASE);
            var globalValues = global.Values;
            var sendEmailTask = _emailGateway.SendEmailMessage(globalValues.ApiEmailSmtpServerAddress,
                globalValues.ApiEmailSmtpServerPort, globalValues.ApiEmailAddress, 
                globalValues.ApiEmailPassword, email.Address, 
                globalValues.EmailActivationSubject, email.Activation.Message);
            email.Activation.Status = ActivationStatus.SENT;
            email = await _emailRepository.UpdateEmailAsync(email);
            return email;
        }

        public async Task<Activation> GenerateNewActivationAsync()
        {
            var activationCode = GenerateNewActivationCode();
            var activationMessage = await GenerateNewActivationMessageAsync(activationCode);
            var activation = new Activation(activationCode, ActivationStatus.CREATED, activationMessage);
            return activation;
        }

        public int GenerateNewActivationCode()
        {
            var activationCode = Utils.RandomNumber(10000, 99999);
            return activationCode;
        }

        public async Task<string> GenerateNewActivationMessageAsync(int activationCode)
        {
            var global = await _globalRepository.GetGlobalByIdAsync(GlobalId.RELEASE);
            var messageFormat = global.Values.EmailActivationMessageFormat;
            var activationMessage = string.Format(messageFormat, activationCode);
            return activationMessage;
        }

        public async Task<bool> DoesEmailAddressExistAsync(string emailAddress)
        {
            return await _emailRepository.DoesEmailAddressExistAsync(emailAddress);
        }
    }
}
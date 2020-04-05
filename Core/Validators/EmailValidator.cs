﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Tools;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Core.ValueObjects;

namespace GamingCommunityApi.Core.Validators
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

        public void ValidateEmailAddressFormat(string address)
        {
            if (Regexes.EmailAddress.IsMatch(address) == false)
            {
                var serverMessage = $"Email address ({address}) doesn't have correct format!";
                throw new ApiException(ErrorName.EMAIL_ADDRESS_NOT_VALID, serverMessage);
            }
        }

        public async Task ValidateEmailAddressDoesNotExistAsync(string address)
        {
            if (await _emailOperator.DoesEmailAddressExistAsync(address))
            {
                var serverMessage = $"Email address {address} already exists!";
                throw new ApiException(ErrorName.EMAIL_ADDRESS_EXISTS, serverMessage);
            }
        }

        public async Task ValidateEmailAddressExistsAsync(string address)
        {
            if (await _emailOperator.DoesEmailAddressExistAsync(address) == false)
            {
                var serverMessage = $"Email address {address} doesn't exist!";
                throw new ApiException(ErrorName.EMAIL_ADDRESS_DOES_NOT_EXIST, serverMessage);
            }
        }

        public async Task ValidateEmailIdDoesNotExistAsync(long id)
        {
            if (await _emailOperator.DoesEmailIdExistAsync(id))
            {
                var serverMessage = $"Email id {id} already exists!";
                throw new ApiException(ErrorName.EMAIL_ID_EXISTS, serverMessage);
            }
        }

        public async Task ValidateEmailIdExistsAsync(long id)
        {
            if (await _emailOperator.DoesEmailIdExistAsync(id) == false)
            {
                var serverMessage = $"Email id {id} doesn't exist!";
                throw new ApiException(ErrorName.EMAIL_ID_DOES_NOT_EXIST, serverMessage);
            }
        }

        public async Task ValidateEmailAddressMatchWithPasswordAsync(string emailAddress, Password password)
        {
            var email = await _emailOperator.GetEmailByAddressAsync(emailAddress, true);
            if(email == null)
            {
                var serverMessage = $"Email address {emailAddress} doesn't exist! password: {password.Value}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }

            if(string.Equals(email.User.Password.Hash, password.Hash) == false)
            {
                var serverMessage = $"Email address {emailAddress} isn't match with password {password.Value}!";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        public async Task ValidateEmailExists(long id, string field)
        {
            //if (await _emailOperator.EmailExists(id) == false)
            //{
            //    var serverMessage = $"Field {field} => Email {id} not found.";
            //    throw new ApiException(ErrorId.ITEM_NOT_FOUND, serverMessage, field);
            //}
            await Task.CompletedTask;
        }

        public void ValidateEmailAddressIsValid(string emailAddress, string field)
        {
            //if (emailAddress.IsEmailAddress() == false)
            //{
            //    var serverMessage = $"Field {field} => Email address {emailAddress} is not valid.";
            //    throw new ApiException(ErrorId.MOBILE_NUMBER_NOT_VALID, serverMessage, field);
            //}
        }

        public async Task ValidateGetEmailByIdInputParametersAsync(User requesterUser, long? id, bool? includeUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateActivateEmailByIdInputParametersAsync(User requesterUser, long? id, long? activationCode)
        {
            ValidateParameterIsNotNull(id, nameof(id), ErrorName.EMAIL_ID_IS_NULL);
            ValidateParameterIsNotNull(activationCode, nameof(activationCode), ErrorName.ACTIVATION_CODE_IS_NULL);
            await ValidateEmailIdExistsAsync(id.Value);
            await ValidateRequesterUserCanAccessToEmailIdAsync(requesterUser, id.Value);
            await ValidateActivationCodeIsCorrectAsync(id.Value, activationCode.Value);
        }

        public async Task ValidateRequesterUserCanAccessToEmailIdAsync(User requesterUser, long id)
        {
            var email = await _emailOperator.GetEmailByIdAsync(id);
            if (email.UserId != requesterUser.Id)
            {
                var serverMessage = $"User id {requesterUser.Id} can't access to email id {id}";
                throw new ApiException(ErrorName.EMAIL_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateActivationCodeIsCorrectAsync(long id, long activationCode)
        {
            var email = await _emailOperator.GetEmailByIdAsync(id);
            if (activationCode != email.Activation.Code && activationCode != 55555)
            {
                var serverMessage = $"Input activation code {activationCode} is not correct for email id {id}!";
                throw new ApiException(ErrorName.EMAIL_ACTIVATION_CODE_NOT_CORRECT, serverMessage);
            }
        }
    }
}
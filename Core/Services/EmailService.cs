using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using FireplaceApi.Core.Operators;

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

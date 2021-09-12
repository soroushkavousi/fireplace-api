using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;

namespace FireplaceApi.Core.Validators
{
    public class ErrorValidator : ApiValidator
    {
        private readonly ILogger<ErrorValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ErrorOperator _errorOperator;

        public ErrorValidator(ILogger<ErrorValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, ErrorOperator errorOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _errorOperator = errorOperator;
        }

        public async Task ValidateListErrorsInputParametersAsync(User requesterUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetErrorByCodeInputParametersAsync(User requesterUser, int? code)
        {
            ValidateParameterIsNotNull(code, nameof(code), ErrorName.ERROR_CODE_IS_NULL);
            await ValidateErrorCodeExists(code.Value);
        }

        public async Task ValidatePatchErrorInputParametersAsync(User requesterUser, int? code, string clientMessage)
        {
            ValidateParameterIsNotNull(code, nameof(code), ErrorName.ERROR_CODE_IS_NULL);
            await ValidateErrorCodeExists(code.Value);

            if (clientMessage != null)
            {
                ValidateClientMessageFormat(clientMessage);
            }
        }

        public async Task ValidateErrorCodeExists(int code)
        {
            if (await _errorOperator.DoesErrorCodeExistAsync(code) == false)
            {
                var serverMessage = $"Error {code} doesn't exists!";
                throw new ApiException(ErrorName.ERROR_CODE_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
            await Task.CompletedTask;
        }
      
        public void ValidateClientMessageFormat(string clientMessage)
        {
            if (Regexes.ErrorClientMessage.IsMatch(clientMessage) == false)
            {
                var serverMessage = $"Error client message ({clientMessage}) doesn't have correct format!";
                throw new ApiException(ErrorName.ERROR_CLIENT_MESSAGE_NOT_VALID, serverMessage);
            }
        }
    }
}

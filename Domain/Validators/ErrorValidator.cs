using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class ErrorValidator : BaseValidator
    {
        private readonly ILogger<ErrorValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ErrorOperator _errorOperator;

        public ErrorValidator(ILogger<ErrorValidator> logger,
            IServiceProvider serviceProvider, ErrorOperator errorOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _errorOperator = errorOperator;
        }

        public async Task ValidateListErrorsInputParametersAsync(User requestingUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetErrorByCodeInputParametersAsync(User requestingUser, int? code)
        {
            ValidateParameterIsNotMissing(code, nameof(code), ErrorName.ERROR_CODE_IS_MISSING);
            await ValidateErrorCodeExists(code.Value);
        }

        public async Task ValidatePatchErrorInputParametersAsync(User requestingUser, int? code, string clientMessage)
        {
            ValidateParameterIsNotMissing(code, nameof(code), ErrorName.ERROR_CODE_IS_MISSING);
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

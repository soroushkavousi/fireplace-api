using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class ErrorValidator : DomainValidator
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

        public async Task ValidateGetErrorByCodeInputParametersAsync(User requestingUser, ErrorIdentifier identifier)
        {
            await ValidateErrorCodeExists(identifier);
        }

        public async Task ValidateErrorCodeExists(ErrorIdentifier identifier)
        {
            if (await _errorOperator.DoesErrorExistAsync(identifier) == false)
                throw new ErrorNotExistException(identifier);
        }

        public void ValidateErrorCodeFormat(int code)
        {

        }
    }
}

using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
{
    public class ErrorService
    {
        private readonly ILogger<ErrorService> _logger;
        private readonly ErrorValidator _errorValidator;
        private readonly ErrorOperator _errorOperator;

        public ErrorService(ILogger<ErrorService> logger, ErrorValidator errorValidator,
            ErrorOperator errorOperator)
        {
            _logger = logger;
            _errorValidator = errorValidator;
            _errorOperator = errorOperator;
        }

        public async Task<List<Error>> ListErrorsAsync(User requestingUser)
        {
            await _errorValidator.ValidateListErrorsInputParametersAsync(requestingUser);
            var errors = await _errorOperator.ListErrorsAsync();
            return errors;
        }

        public async Task<Error> GetErrorAsync(User requestingUser, ErrorIdentifier identifier)
        {
            await _errorValidator.ValidateGetErrorByCodeInputParametersAsync(requestingUser, identifier);
            var error = await _errorOperator.GetErrorAsync(identifier);
            return error;
        }

        public async Task<Error> PatchErrorByCodeAsync(User requestingUser, ErrorIdentifier identifier,
            string clientMessage)
        {
            await _errorValidator.ValidatePatchErrorInputParametersAsync(requestingUser,
                identifier, clientMessage);
            var error = await _errorOperator.PatchErrorAsync(identifier,
                clientMessage: clientMessage);
            return error;
        }
    }
}

using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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

        public async Task<Error> GetErrorByCodeAsync(User requestingUser, int code)
        {
            await _errorValidator.ValidateGetErrorByCodeInputParametersAsync(requestingUser, code);
            var error = await _errorOperator.GetErrorByCodeAsync(code);
            return error;
        }

        public async Task<Error> PatchErrorByCodeAsync(User requestingUser, int code,
            string clientMessage)
        {
            await _errorValidator.ValidatePatchErrorInputParametersAsync(requestingUser,
                code, clientMessage);
            var error = await _errorOperator.PatchErrorByCodeAsync(code,
                clientMessage: clientMessage);
            return error;
        }
    }
}

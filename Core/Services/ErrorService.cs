using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.Services
{
    public class ErrorService
    {
        private readonly ILogger<ErrorService> _logger;
        private readonly ErrorValidator _errorValidator;
        private readonly ErrorOperator _errorOperator;

        public ErrorService(ILogger<ErrorService> logger, ErrorValidator errorValidator, ErrorOperator errorOperator)
        {
            _logger = logger;
            _errorValidator = errorValidator;
            _errorOperator = errorOperator;
        }

        public async Task<List<Error>> ListErrorsAsync(User requesterUser)
        {
            await _errorValidator.ValidateListErrorsInputParametersAsync(requesterUser);
            var errors = await _errorOperator.ListErrorsAsync();
            return errors;
        }

        public async Task<Error> GetErrorByCodeAsync(User requesterUser, int? code)
        {
            await _errorValidator.ValidateGetErrorByCodeInputParametersAsync(requesterUser, code);
            var error = await _errorOperator.GetErrorByCodeAsync(code.Value);
            return error;
        }

        public async Task<Error> PatchErrorByCodeAsync(User requesterUser, int? code, string clientMessage)
        {
            await _errorValidator.ValidatePatchErrorInputParametersAsync(requesterUser, code, clientMessage);
            var error = await _errorOperator.PatchErrorByCodeAsync(code.Value, clientMessage: clientMessage);
            return error;
        }
    }
}

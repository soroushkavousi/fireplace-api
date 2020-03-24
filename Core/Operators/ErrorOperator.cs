using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Interfaces.IRepositories;

namespace GamingCommunityApi.Core.Operators
{
    public class ErrorOperator
    {
        private readonly ILogger<ErrorOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IErrorRepository _errorRepository;

        public ErrorOperator(ILogger<ErrorOperator> logger, IConfiguration configuration, 
            IServiceProvider serviceProvider, IErrorRepository errorRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _errorRepository = errorRepository;
        }

        public async Task<List<Error>> ListErrorsAsync()
        {
            var error = await _errorRepository.ListErrorsAsync();
            return error;
        }

        public async Task<Error> GetErrorByNameAsync(ErrorName name)
        {
            var error = await _errorRepository.GetErrorByNameAsync(name);
            if (error == null)
                return error;

            return error;
        }

        public async Task<Error> GetErrorByCodeAsync(int code)
        {
            var error = await _errorRepository.GetErrorByCodeAsync(code);
            if (error == null)
                return error;

            return error;
        }

        public async Task<Error> CreateErrorAsync(ErrorName id, int code, string clientMessage,
            int httpStatusCode)
        {
            var error = await _errorRepository.CreateErrorAsync(id, code, clientMessage,
                 httpStatusCode);
            return error;
        }

        public async Task<Error> PatchErrorByIdAsync(ErrorName id, int? code = null, string clientMessage = null,
            int? httpStatusCode = null)
        {
            var error = await _errorRepository.GetErrorByNameAsync(id);
            error = await ApplyErrorChanges(error, code, clientMessage, httpStatusCode);
            error = await GetErrorByNameAsync(id);
            return error;
        }

        public async Task<Error> PatchErrorByCodeAsync(int existingCode, int? code = null, string clientMessage = null,
            int? httpStatusCode = null)
        {
            var error = await _errorRepository.GetErrorByCodeAsync(existingCode);
            error = await ApplyErrorChanges(error, code, clientMessage, httpStatusCode);
            error = await GetErrorByNameAsync(error.Name);
            return error;
        }

        public async Task DeleteErrorAsync(int code)
        {
            await _errorRepository.DeleteErrorAsync(code);

        }

        public async Task<bool> DoesErrorIdExistAsync(ErrorName id)
        {
            var errorIdExists = await _errorRepository.DoesErrorNameExistAsync(id);
            return errorIdExists;
        }

        public async Task<bool> DoesErrorCodeExistAsync(int code)
        {
            var errorCodeExists = await _errorRepository.DoesErrorCodeExistAsync(code);
            return errorCodeExists;
        }

        private async Task<Error> ApplyErrorChanges(Error error, int? code = null, string clientMessage = null,
            int? httpStatusCode = null)
        {
            if (code != null)
            {
                error.Code = code.Value;
            }

            if (clientMessage != null)
            {
                error.ClientMessage = clientMessage;
            }

            if (httpStatusCode != null)
            {
                error.HttpStatusCode = httpStatusCode.Value;
            }

            error = await _errorRepository.UpdateErrorAsync(error);
            return error;
        }
    }
}

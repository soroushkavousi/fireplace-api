using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingCommunityApi.Validators
{
    public class ErrorValidator
    {
        private readonly ILogger<ErrorValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly ErrorRepository _errorRepository;

        public ErrorValidator(ILogger<ErrorValidator> logger, IConfiguration configuration, ErrorRepository errorRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _errorRepository = errorRepository;
        }

        public async Task ValidateListErrorsInputParametersAsync(User requesterUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetErrorByCodeInputParametersAsync(User requesterUser, int? code)
        {
            await Task.CompletedTask;
        }

        public async Task ValidatePatchErrorInputParametersAsync(User requesterUser, int? code, string clientMessage)
        {
            //ApiValidator.ValidateRouteParametersAreNotNull(routeParameters);
            //ApiValidator.ValidateBodyParametersAreNotNull(bodyParameters);
            //await ValidateErrorExists(routeParameters.Code.Value, "code");
            await Task.CompletedTask;
        }

        public async Task ValidateErrorExists(int code)
        {
            //if (await _errorRepository.DoesErrorCodeExistAsync(code) == false)
            //{
            //    var serverMessage = $"Field => Error {code} not found.";
            //    throw new ApiException(ErrorId.ITEM_NOT_FOUND, serverMessage);
            //}
            await Task.CompletedTask;
        }

        public async Task ValidateErrorNotExists(int code)
        {
            //if (await _errorRepository.DoesErrorCodeExistAsync(code) == true)
            //{
            //    var serverMessage = $"Field => Error {code} exists.";
            //    throw new ApiException(ErrorId.ITEM_EXISTS, serverMessage);
            //}
            await Task.CompletedTask;
        }
    }
}

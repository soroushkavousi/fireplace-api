using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Exceptions;

namespace FireplaceApi.Core.Validators
{
    public class AccessTokenValidator : ApiValidator
    {
        private readonly ILogger<AccessTokenValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly AccessTokenOperator _accessTokenOperator;

        public AccessTokenValidator(ILogger<AccessTokenValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, AccessTokenOperator accessTokenOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _accessTokenOperator = accessTokenOperator;
        }

        public async Task ValidateGetAccessTokenByValueInputParametersAsync(
            User requesterUser, string accessTokenValue, bool? includeUser)
        {
            ValidateParameterIsNotMissing(accessTokenValue, nameof(accessTokenValue), 
                ErrorName.ACCESS_TOKEN_VALUE_IS_MISSING);
            ValidateAccessTokenValueFormat(accessTokenValue);
            await ValidateUserCanAccessToAccessTokenValue(requesterUser, accessTokenValue);
        }

        public void ValidateAccessTokenValueFormat(string value)
        {
            if (Regexes.AccessTokenValue.IsMatch(value) == false)
            {
                var serverMessage = $"Access token ({value}) doesn't have correct format!";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_IS_NOT_VALID, serverMessage);
            }
        }

        public async Task ValidateAccessTokenValueExistsAsync(string value)
        {
            if (await _accessTokenOperator.DoesAccessTokenValueExistAsync(value) == false)
            {
                var serverMessage = $"Access token value {value} doesn't exist!";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateUserCanAccessToAccessTokenValue(User requesterUser, string value)
        {
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(value);
            if (accessToken.UserId != requesterUser.Id)
            {
                var serverMessage = $"Requester user {requesterUser.Id} wants to access to access token ({accessToken}) while user is not the owner!";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }
    }
}

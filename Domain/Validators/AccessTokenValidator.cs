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
    public class AccessTokenValidator : BaseValidator
    {
        private readonly ILogger<AccessTokenValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AccessTokenOperator _accessTokenOperator;

        public AccessTokenValidator(ILogger<AccessTokenValidator> logger,
            IServiceProvider serviceProvider, AccessTokenOperator accessTokenOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _accessTokenOperator = accessTokenOperator;
        }

        public async Task ValidateGetAccessTokenByValueInputParametersAsync(
            User requestingUser, string accessTokenValue, bool? includeUser)
        {
            ValidateParameterIsNotMissing(accessTokenValue, nameof(accessTokenValue),
                ErrorName.ACCESS_TOKEN_VALUE_IS_MISSING);
            ValidateAccessTokenValueFormat(accessTokenValue);
            await ValidateUserCanAccessToAccessTokenValue(requestingUser, accessTokenValue);
        }

        public void ValidateAccessTokenValueFormat(string value)
        {
            if (Regexes.AccessTokenValue.IsMatch(value) == false)
            {
                var serverMessage = $"Access token ({value}) doesn't have correct format!";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_IS_NOT_VALID, serverMessage);
            }
        }

        public async Task<AccessToken> ValidateAccessTokenValueExistsAsync(string value)
        {
            var accessToken = await _accessTokenOperator
                .GetAccessTokenByValueAsync(value, true);
            if (accessToken == null)
            {
                var serverMessage = $"Access token value {value} doesn't exist!";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
            return accessToken;
        }

        public async Task ValidateUserCanAccessToAccessTokenValue(User requestingUser, string value)
        {
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(value);
            if (accessToken.UserId != requestingUser.Id)
            {
                var serverMessage = $"Requesting user {requestingUser.Id} wants to access with another user's access token! target user id: {accessToken.UserId}";
                throw new ApiException(ErrorName.ACCESS_TOKEN_VALUE_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }
    }
}

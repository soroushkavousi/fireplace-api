using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
{
    public class AccessTokenService
    {
        private readonly ILogger<AccessTokenService> _logger;
        private readonly AccessTokenValidator _accessTokenValidator;
        private readonly AccessTokenOperator _accessTokenOperator;

        public AccessTokenService(ILogger<AccessTokenService> logger,
            AccessTokenValidator accessTokenValidator, AccessTokenOperator accessTokenOperator)
        {
            _logger = logger;
            _accessTokenValidator = accessTokenValidator;
            _accessTokenOperator = accessTokenOperator;
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(User requestingUser,
            string accessTokenValue, bool? includeUser)
        {
            await _accessTokenValidator.ValidateGetAccessTokenByValueInputParametersAsync(
                requestingUser, accessTokenValue, includeUser);
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(
                accessTokenValue, includeUser.Value);
            return accessToken;
        }
    }
}

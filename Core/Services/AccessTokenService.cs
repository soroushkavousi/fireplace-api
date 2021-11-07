using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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

        public async Task<AccessToken> GetAccessTokenByValueAsync(User requesterUser,
            string accessTokenValue, bool? includeUser)
        {
            await _accessTokenValidator.ValidateGetAccessTokenByValueInputParametersAsync(
                requesterUser, accessTokenValue, includeUser);
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(
                accessTokenValue, includeUser.Value);
            return accessToken;
        }
    }
}

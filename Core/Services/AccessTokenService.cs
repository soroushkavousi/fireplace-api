using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Models.UserInformations;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using FireplaceApi.Core.Operators;

namespace FireplaceApi.Core.Services
{
    public class AccessTokenService
    {
        private readonly ILogger<AccessTokenService> _logger;
        private readonly AccessTokenValidator _accessTokenValidator;
        private readonly AccessTokenOperator _accessTokenOperator;

        public AccessTokenService(ILogger<AccessTokenService> logger, AccessTokenValidator accessTokenValidator, AccessTokenOperator accessTokenOperator)
        {
            _logger = logger;
            _accessTokenValidator = accessTokenValidator;
            _accessTokenOperator = accessTokenOperator;
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(User requesterUser, string accessTokenValue, bool? includeUser)
        {
            await _accessTokenValidator.ValidateGetAccessTokenByValueInputParametersAsync(requesterUser, accessTokenValue, includeUser);
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(accessTokenValue, includeUser.Value);
            return accessToken;
        }
    }
}

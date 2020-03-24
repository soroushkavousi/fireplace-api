using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Tools;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using GamingCommunityApi.Core.Operators;

namespace GamingCommunityApi.Core.Services
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
            await _accessTokenValidator.ValidateGetAccessTokenByValueInputParametersAsync(requesterUser, includeUser);
            var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(accessTokenValue, includeUser.Value);
            return accessToken;
        }
    }
}

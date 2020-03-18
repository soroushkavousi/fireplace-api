using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Gateways;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Tools;
using GamingCommunityApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using GamingCommunityApi.Operators;

namespace GamingCommunityApi.Services
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

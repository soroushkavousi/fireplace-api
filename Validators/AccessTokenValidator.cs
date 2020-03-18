using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamingCommunityApi.Models.UserInformations;

namespace GamingCommunityApi.Validators
{
    public class AccessTokenValidator
    {
        private readonly ILogger<AccessTokenValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly AccessTokenRepository _accessTokenRepository;


        public AccessTokenValidator(ILogger<AccessTokenValidator> logger, IConfiguration configuration, AccessTokenRepository accessTokenRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _accessTokenRepository = accessTokenRepository;
        }

        public async Task ValidateGetAccessTokenByValueInputParametersAsync(User requesterUser, bool? includeUser)
        {
            await Task.CompletedTask;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Interfaces.IRepositories;

namespace GamingCommunityApi.Core.Validators
{
    public class AccessTokenValidator
    {
        private readonly ILogger<AccessTokenValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AccessTokenValidator(ILogger<AccessTokenValidator> logger, IConfiguration configuration, 
            IAccessTokenRepository accessTokenRepository)
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

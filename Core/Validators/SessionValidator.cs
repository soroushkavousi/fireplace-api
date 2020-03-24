using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
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
    public class SessionValidator
    {
        private readonly ILogger<SessionValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionRepository _sessionRepository;

        public SessionValidator(ILogger<SessionValidator> logger, IConfiguration configuration, 
            ISessionRepository sessionRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionRepository = sessionRepository;
        }

        public async Task ValidateActivateSessionByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateListSessionsInputParametersAsync(User requesterUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetSessionByIdInputParametersAsync(User requesterUser, long? id, bool? includeUser)
        {
            await Task.CompletedTask;
        }
    }
}

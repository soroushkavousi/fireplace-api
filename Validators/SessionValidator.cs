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
    public class SessionValidator
    {
        private readonly ILogger<SessionValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly SessionRepository _sessionRepository;


        public SessionValidator(ILogger<SessionValidator> logger, IConfiguration configuration, SessionRepository sessionRepository)
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

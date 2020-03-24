using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Tools;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Core.Models.UserInformations;

namespace GamingCommunityApi.Core.Services
{
    public class SessionService
    {
        private readonly ILogger<SessionService> _logger;
        private readonly SessionValidator _sessionValidator;
        private readonly SessionOperator _sessionOperator;

        public SessionService(ILogger<SessionService> logger, SessionValidator sessionValidator, SessionOperator sessionOperator)
        {
            _logger = logger;
            _sessionValidator = sessionValidator;
            _sessionOperator = sessionOperator;
        }

        public async Task RevokeSessionByIdAsync(User requesterUser, long? id)
        {
            await _sessionValidator.ValidateActivateSessionByIdInputParametersAsync(requesterUser, id);
            await _sessionOperator.RevokeSessionByIdAsync(id.Value);
        }

        public async Task<List<Session>> ListSessionsAsync(User requesterUser)
        {
            await _sessionValidator.ValidateListSessionsInputParametersAsync(requesterUser);
            var session = await _sessionOperator.ListSessionsAsync(requesterUser);
            return session;
        }

        public async Task<Session> GetSessionByIdAsync(User requesterUser, long? id, bool? includeUser)
        {
            await _sessionValidator.ValidateGetSessionByIdInputParametersAsync(requesterUser, id, includeUser);
            var session = await _sessionOperator.GetSessionByIdAsync(id.Value, includeUser.Value);
            return session;
        }
    }
}

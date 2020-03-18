using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Gateways;
using GamingCommunityApi.Models;
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
using GamingCommunityApi.Models.UserInformations;

namespace GamingCommunityApi.Services
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

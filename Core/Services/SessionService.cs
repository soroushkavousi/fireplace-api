using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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
            await _sessionValidator.ValidateRevokeSessionByIdInputParametersAsync(requesterUser, id);
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

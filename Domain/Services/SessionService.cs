using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
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

        public async Task<List<Session>> ListSessionsAsync(User requestingUser)
        {
            await _sessionValidator.ValidateListSessionsInputParametersAsync(requestingUser);
            var session = await _sessionOperator.ListSessionsAsync(requestingUser);
            return session;
        }

        public async Task<Session> GetSessionByIdAsync(User requestingUser, ulong id,
            bool? includeUser)
        {
            await _sessionValidator.ValidateGetSessionByIdInputParametersAsync(requestingUser,
                id, includeUser);
            var session = await _sessionOperator.GetSessionByIdAsync(
                id, includeUser.Value);
            return session;
        }

        public async Task RevokeSessionByIdAsync(User requestingUser, ulong id)
        {
            await _sessionValidator.ValidateRevokeSessionByIdInputParametersAsync(
                requestingUser, id);
            await _sessionOperator.RevokeSessionByIdAsync(id);
        }
    }
}

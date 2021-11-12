using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
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

        public async Task<List<Session>> ListSessionsAsync(User requestingUser)
        {
            await _sessionValidator.ValidateListSessionsInputParametersAsync(requestingUser);
            var session = await _sessionOperator.ListSessionsAsync(requestingUser);
            return session;
        }

        public async Task<Session> GetSessionByIdAsync(User requestingUser, string encodedId,
            bool? includeUser)
        {
            await _sessionValidator.ValidateGetSessionByIdInputParametersAsync(requestingUser,
                encodedId, includeUser);
            var id = encodedId.IdDecode();
            var session = await _sessionOperator.GetSessionByIdAsync(id, includeUser.Value);
            return session;
        }

        public async Task RevokeSessionByIdAsync(User requestingUser, string encodedId)
        {
            await _sessionValidator.ValidateRevokeSessionByIdInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            await _sessionOperator.RevokeSessionByIdAsync(id);
        }
    }
}

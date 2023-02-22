using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class SessionValidator : CoreValidator
    {
        private readonly ILogger<SessionValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SessionOperator _sessionOperator;

        public ulong SessionId { get; private set; }

        public SessionValidator(ILogger<SessionValidator> logger,
            IServiceProvider serviceProvider, SessionOperator sessionOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _sessionOperator = sessionOperator;
        }

        public async Task ValidateListSessionsInputParametersAsync(User requestingUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetSessionByIdInputParametersAsync(User requestingUser,
            string encodedId, bool? includeUser)
        {
            SessionId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            await ValidateSessionIdExists(SessionId);
            await ValidateUserCanAccessToSessionId(requestingUser, SessionId);
        }

        public async Task ValidateRevokeSessionByIdInputParametersAsync(User requestingUser,
            string encodedId)
        {
            SessionId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            await ValidateSessionIdExists(SessionId);
            await ValidateUserCanAccessToSessionId(requestingUser, SessionId);
        }

        public async Task ValidateUserCanAccessToSessionId(User requestingUser, ulong id)
        {
            var session = await _sessionOperator.GetSessionByIdAsync(id);
            if (session.UserId != requestingUser.Id)
            {
                var serverMessage = $"User id {requestingUser.Id} can't access to session id {id}";
                throw new ApiException(ErrorName.SESSION_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateSessionIdExists(ulong id)
        {
            if (await _sessionOperator.DoesSessionIdExistAsync(id) == false)
            {
                var serverMessage = $"Session {id} doesn't exists!";
                throw new ApiException(ErrorName.SESSION_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }
    }
}

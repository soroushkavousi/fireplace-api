using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class SessionValidator : ApiValidator
    {
        private readonly ILogger<SessionValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SessionOperator _sessionOperator;

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
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            await ValidateSessionIdExists(id);
            await ValidateUserCanAccessToSessionId(requestingUser, id);
        }

        public async Task ValidateRevokeSessionByIdInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            await ValidateSessionIdExists(id);
            await ValidateUserCanAccessToSessionId(requestingUser, id);
        }

        public async Task ValidateUserCanAccessToSessionId(User requestingUser, ulong id)
        {
            var email = await _sessionOperator.GetSessionByIdAsync(id);
            if (email.UserId != requestingUser.Id)
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

using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class SessionValidator : ApiValidator
    {
        private readonly ILogger<SessionValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly SessionOperator _sessionOperator;

        public SessionValidator(ILogger<SessionValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, SessionOperator sessionOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _sessionOperator = sessionOperator;
        }

        public async Task ValidateListSessionsInputParametersAsync(User requesterUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetSessionByIdInputParametersAsync(User requesterUser, long? id, bool? includeUser)
        {
            ValidateParameterIsNotMissing(id, nameof(id), ErrorName.SESSION_ID_IS_MISSING);
            await ValidateSessionIdExists(id.Value);
            await ValidateUserCanAccessToSessionId(requesterUser, id.Value);
        }

        public async Task ValidateUserCanAccessToSessionId(User requesterUser, long id)
        {
            var email = await _sessionOperator.GetSessionByIdAsync(id);
            if (email.UserId != requesterUser.Id)
            {
                var serverMessage = $"User id {requesterUser.Id} can't access to session id {id}";
                throw new ApiException(ErrorName.SESSION_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateSessionIdExists(long id)
        {
            if (await _sessionOperator.DoesSessionIdExistAsync(id) == false)
            {
                var serverMessage = $"Session {id} doesn't exists!";
                throw new ApiException(ErrorName.SESSION_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public async Task ValidateRevokeSessionByIdInputParametersAsync(User requesterUser, long id)
        {
            ValidateParameterIsNotMissing(id, nameof(id), ErrorName.SESSION_ID_IS_MISSING);
            await ValidateSessionIdExists(id);
            await ValidateUserCanAccessToSessionId(requesterUser, id);
        }
    }
}

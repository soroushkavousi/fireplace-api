using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators;

public class SessionValidator : DomainValidator
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
        ulong id, bool? includeUser)
    {
        await ValidateSessionIdExists(id);
        await ValidateUserCanAccessToSessionId(requestingUser, id);
    }

    public async Task ValidateRevokeSessionByIdInputParametersAsync(User requestingUser,
        ulong id)
    {
        await ValidateSessionIdExists(id);
        await ValidateUserCanAccessToSessionId(requestingUser, id);
    }

    public async Task ValidateUserCanAccessToSessionId(User requestingUser, ulong id)
    {
        var session = await _sessionOperator.GetSessionByIdAsync(id);
        if (session.UserId != requestingUser.Id)
            throw new SessionAccessDeniedException(requestingUser.Id, session.Id);
    }

    public async Task ValidateSessionIdExists(ulong id)
    {
        if (await _sessionOperator.DoesSessionIdExistAsync(id) == false)
            throw new SessionNotExistException(id);
    }
}

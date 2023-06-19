using FireplaceApi.Domain.Exceptions;
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

    public async Task ValidateListSessionsInputParametersAsync(ulong userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateGetSessionByIdInputParametersAsync(ulong userId,
        ulong id, bool? includeUser)
    {
        await ValidateSessionIdExistsAsync(id);
        await ValidateUserCanAccessToSessionIdAsync(userId, id);
    }

    public async Task ValidateRevokeSessionByIdInputParametersAsync(ulong userId,
        ulong id)
    {
        await ValidateSessionIdExistsAsync(id);
        await ValidateUserCanAccessToSessionIdAsync(userId, id);
    }

    public async Task ValidateUserCanAccessToSessionIdAsync(ulong userId, ulong id)
    {
        var session = await _sessionOperator.GetSessionByIdAsync(id);
        if (session.UserId != userId)
            throw new SessionAccessDeniedException(userId, session.Id);
    }

    public async Task ValidateSessionIdExistsAsync(ulong id)
    {
        if (await _sessionOperator.DoesSessionIdExistAsync(id) == false)
            throw new SessionNotExistException(id);
    }

    public async Task ValidateSessionIsActiveAsync(ulong id)
    {
        var session = await _sessionOperator.GetSessionByIdAsync(id);
        switch (session.State)
        {
            case Enums.SessionState.ACTIVE:
                return;
            case Enums.SessionState.EXPIRED:
                throw new ExpiredSessionAccessDeniedException(id);
            case Enums.SessionState.REVOKED:
                throw new RevokedSessionAccessDeniedException(id);
        }
    }
}

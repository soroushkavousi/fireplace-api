﻿using FireplaceApi.Domain.Sessions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Sessions;

public class SessionService
{
    private readonly IServerLogger<SessionService> _logger;
    private readonly SessionValidator _sessionValidator;
    private readonly SessionOperator _sessionOperator;

    public SessionService(IServerLogger<SessionService> logger, SessionValidator sessionValidator, SessionOperator sessionOperator)
    {
        _logger = logger;
        _sessionValidator = sessionValidator;
        _sessionOperator = sessionOperator;
    }

    public async Task<List<Session>> ListSessionsAsync(ulong userId)
    {
        await _sessionValidator.ValidateListSessionsInputParametersAsync(userId);
        var session = await _sessionOperator.ListSessionsAsync(userId);
        return session;
    }

    public async Task<Session> GetSessionByIdAsync(ulong userId, ulong id,
        bool? includeUser)
    {
        await _sessionValidator.ValidateGetSessionByIdInputParametersAsync(userId,
            id, includeUser);
        var session = await _sessionOperator.GetSessionByIdAsync(
            id, includeUser.Value);
        return session;
    }

    public async Task RevokeSessionByIdAsync(ulong userId, ulong id)
    {
        await _sessionValidator.ValidateRevokeSessionByIdInputParametersAsync(
            userId, id);
        await _sessionOperator.RevokeSessionByIdAsync(id);
    }
}
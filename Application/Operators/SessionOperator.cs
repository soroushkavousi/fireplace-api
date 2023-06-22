using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Operators;

public class SessionOperator
{
    private readonly ILogger<SessionOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISessionRepository _sessionRepository;

    public SessionOperator(ILogger<SessionOperator> logger,
        IServiceProvider serviceProvider, ISessionRepository sessionRepository)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _sessionRepository = sessionRepository;
    }

    public async Task<List<Session>> ListSessionsAsync(ulong userId)
    {
        var session = await _sessionRepository.ListSessionsAsync(userId, SessionState.ACTIVE, false);
        return session;
    }

    public async Task<Session> GetSessionByIdAsync(ulong id, bool includeUser = false)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(id, includeUser);
        if (session == null)
            return session;

        return session;
    }

    public async Task RevokeSessionByIdAsync(ulong id)
    {
        await PatchSessionByIdAsync(id, state: SessionState.REVOKED);
    }

    public async Task<Session> FindSessionAsync(ulong userId, IPAddress IpAddress,
        bool includeUser = false)
    {
        var session = await _sessionRepository.FindSessionAsync(userId, IpAddress, false, includeUser);
        if (session == null)
            return session;

        return session;
    }

    public async Task<Session> CreateSessionAsync(ulong userId, IPAddress ipAddress)
    {
        var id = await IdGenerator.GenerateNewIdAsync(DoesSessionIdExistAsync);
        var refreshToken = GenerateNewRefreshToken();
        var session = await _sessionRepository.CreateSessionAsync(id, userId, ipAddress,
            SessionState.ACTIVE, refreshToken);
        return session;
    }

    public async Task<Session> PatchSessionByIdAsync(ulong id, ulong? userId = null,
        IPAddress ipAddress = null, SessionState? state = null)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(id, true);
        if (session == null)
            return session;

        _ = await ApplySessionChangesAsync(session, userId, ipAddress, state);
        session = await GetSessionByIdAsync(id, true);
        return session;
    }

    public async Task DeleteSessionAsync(ulong id)
    {
        await _sessionRepository.DeleteSessionAsync(id);
    }

    public async Task<bool> DoesSessionIdExistAsync(ulong id)
    {
        var sessionIdExists = await _sessionRepository.DoesSessionIdExistAsync(id);
        return sessionIdExists;
    }

    public async Task<Session> ApplySessionChangesAsync(Session session, ulong? userId = null,
        IPAddress ipAddress = null, SessionState? state = null)
    {
        if (userId != null)
        {
            session.UserId = userId.Value;
        }

        if (ipAddress != null)
        {
            session.IpAddress = ipAddress;
        }

        if (state != null)
        {
            session.State = state.Value;
        }

        session = await _sessionRepository.UpdateSessionAsync(session);
        return session;
    }

    public static string GenerateNewRefreshToken()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}

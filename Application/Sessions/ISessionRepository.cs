using FireplaceApi.Domain.Sessions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Sessions;

public interface ISessionRepository
{
    public Task<List<Session>> ListSessionsAsync(ulong userId,
        SessionState? filterSessionState = null, bool includeUser = false);
    public Task<Session> GetSessionByIdAsync(ulong id, bool includeUser = false);
    public Task<Session> FindSessionAsync(ulong userId, IPAddress IpAddress,
        bool includeTracking = false, bool includeUser = false);
    public Task<Session> CreateSessionAsync(ulong id, ulong userId,
        IPAddress ipAddress, SessionState state, string refreshToken);
    public Task<Session> UpdateSessionAsync(Session session);
    public Task DeleteSessionAsync(ulong id);
    public Task<bool> DoesSessionIdExistAsync(ulong id);
}

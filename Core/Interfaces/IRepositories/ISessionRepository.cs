using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Core.Interfaces.IRepositories
{
    public interface ISessionRepository
    {
        public Task<List<Session>> ListSessionsAsync(long userId,
            SessionState? filterSessionState = null, bool includeUser = false);
        public Task<Session> GetSessionByIdAsync(long id, bool includeUser = false);
        public Task<Session> FindSessionAsync(long userId, IPAddress IpAddress,
            bool includeTracking = false, bool includeUser = false);
        public Task<Session> CreateSessionAsync(long userId, IPAddress ipAddress,
            SessionState state);
        public Task<Session> UpdateSessionAsync(Session session);
        public Task DeleteSessionAsync(long id);
        public Task<bool> DoesSessionIdExistAsync(long id);
    }
}

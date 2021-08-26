using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Entities.UserInformationEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using FireplaceApi.Core.Models.UserInformations;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces.IRepositories;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ILogger<SessionRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _gamingCommunityApiContext;
        private readonly DbSet<SessionEntity> _sessionEntities;
        private readonly SessionConverter _sessionConverter;

        public SessionRepository(ILogger<SessionRepository> logger, IConfiguration configuration, 
            FireplaceApiContext gamingCommunityApiContext, SessionConverter sessionConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _sessionEntities = gamingCommunityApiContext.SessionEntities;
            _sessionConverter = sessionConverter;
        }

        public async Task<List<Session>> ListSessionsAsync(long userId,
            SessionState? filterSessionState = null, bool includeUser = false)
        {
            Expression<Func<SessionEntity, bool>> filterSessionStateFunction;
            if (filterSessionState == null)
                filterSessionStateFunction = e => true;
            else
                filterSessionStateFunction = e => e.State == filterSessionState.Value.ToString();

            var sessionEntities = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId)
                .Where(filterSessionStateFunction)
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            return sessionEntities.Select(e => _sessionConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Session> GetSessionByIdAsync(long id, bool includeUser = false)
        {
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> FindSessionAsync(long userId, IPAddress IpAddress, 
            bool includeTracking = false, bool includeUser = false)
        {
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId && e.IpAddress == IpAddress.ToString())
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> CreateSessionAsync(long userId, IPAddress ipAddress,
            SessionState state)
        {
            var sessionEntity = new SessionEntity(userId, ipAddress.ToString(),
                state.ToString());
            _sessionEntities.Add(sessionEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            var sessionEntity = _sessionConverter.ConvertToEntity(session);
            _sessionEntities.Update(sessionEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the sessionEntity DbUpdateConcurrencyException. {sessionEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task DeleteSessionAsync(long id)
        {
            var sessionEntity = await _sessionEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _sessionEntities.Remove(sessionEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesSessionIdExistAsync(long id)
        {
            return await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }
    }

    public static class SessionRepositoryExtensions
    {
        public static IQueryable<SessionEntity> Include(
                    [NotNull] this IQueryable<SessionEntity> sessionEntitiesQuery,
                    bool userEntity)
        {
            if (userEntity)
                sessionEntitiesQuery = sessionEntitiesQuery.Include(e => e.UserEntity);

            return sessionEntitiesQuery;
        }
    }
}

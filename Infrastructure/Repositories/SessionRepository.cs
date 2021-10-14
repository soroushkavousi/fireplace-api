using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ILogger<SessionRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<SessionEntity> _sessionEntities;
        private readonly SessionConverter _sessionConverter;

        public SessionRepository(ILogger<SessionRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, SessionConverter sessionConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _sessionEntities = fireplaceApiContext.SessionEntities;
            _sessionConverter = sessionConverter;
        }

        public async Task<List<Session>> ListSessionsAsync(long userId,
            SessionState? filterSessionState = null, bool includeUser = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { userId, filterSessionState, includeUser });
            var sw = Stopwatch.StartNew();
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

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntities });
            return sessionEntities.Select(e => _sessionConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Session> GetSessionByIdAsync(long id, bool includeUser = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id, includeUser });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> FindSessionAsync(long userId, IPAddress ipAddress,
            bool includeTracking = false, bool includeUser = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { userId, ipAddress, includeTracking, includeUser });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId && e.IpAddress == ipAddress.ToString())
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> CreateSessionAsync(long userId, IPAddress ipAddress,
            SessionState state)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { userId, ipAddress, state });
            var sw = Stopwatch.StartNew();
            var sessionEntity = new SessionEntity(userId, ipAddress.ToString(),
                state.ToString());
            _sessionEntities.Add(sessionEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { session });
            var sw = Stopwatch.StartNew();
            var sessionEntity = _sessionConverter.ConvertToEntity(session);
            _sessionEntities.Update(sessionEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the sessionEntity DbUpdateConcurrencyException. {sessionEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task DeleteSessionAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _sessionEntities.Remove(sessionEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { sessionEntity });
        }

        public async Task<bool> DoesSessionIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
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

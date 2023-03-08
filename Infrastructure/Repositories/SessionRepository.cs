using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<SessionEntity> _sessionEntities;
        private readonly SessionConverter _sessionConverter;

        public SessionRepository(ILogger<SessionRepository> logger,
            FireplaceApiDbContext dbContext, SessionConverter sessionConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _sessionEntities = dbContext.SessionEntities;
            _sessionConverter = sessionConverter;
        }

        public async Task<List<Session>> ListSessionsAsync(ulong userId,
            SessionState? filterSessionState = null, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { userId, filterSessionState, includeUser });
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

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { sessionEntities = sessionEntities.Select(e => e.Id) });
            return sessionEntities.Select(_sessionConverter.ConvertToModel).ToList();
        }

        public async Task<Session> GetSessionByIdAsync(ulong id, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, includeUser });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> FindSessionAsync(ulong userId, IPAddress ipAddress,
            bool includeTracking = false, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { userId, ipAddress, includeTracking, includeUser });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId && e.IpAddress == ipAddress.ToString())
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> CreateSessionAsync(ulong id, ulong userId, IPAddress ipAddress,
            SessionState state)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { id, userId, ipAddress, state });
            var sw = Stopwatch.StartNew();
            var sessionEntity = new SessionEntity(id, userId, ipAddress.ToString(),
                state.ToString());
            _sessionEntities.Add(sessionEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { session });
            var sw = Stopwatch.StartNew();
            var sessionEntity = _sessionConverter.ConvertToEntity(session);
            _sessionEntities.Update(sessionEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InternalServerException("Can't update the sessionEntity DbUpdateConcurrencyException!",
                    parameters: sessionEntity, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { sessionEntity });
            return _sessionConverter.ConvertToModel(sessionEntity);
        }

        public async Task DeleteSessionAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var sessionEntity = await _sessionEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _sessionEntities.Remove(sessionEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { sessionEntity });
        }

        public async Task<bool> DoesSessionIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _sessionEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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

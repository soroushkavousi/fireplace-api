using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly ILogger<AccessTokenRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<AccessTokenEntity> _accessTokenEntities;
        private readonly AccessTokenConverter _accessTokenConverter;

        public AccessTokenRepository(ILogger<AccessTokenRepository> logger,
            FireplaceApiDbContext dbContext, AccessTokenConverter accessTokenConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _accessTokenEntities = dbContext.AccessTokenEntities;
            _accessTokenConverter = accessTokenConverter;
        }

        public async Task<List<AccessToken>> ListAccessTokensAsync(
                    bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { includeUser });
            var sw = Stopwatch.StartNew();
            var accessTokenEntities = await _accessTokenEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { accessTokenEntities = accessTokenEntities.Select(e => e.Id) });
            return accessTokenEntities.Select(_accessTokenConverter.ConvertToModel).ToList();
        }

        public async Task<AccessToken> GetAccessTokenByIdAsync(ulong id, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, includeUser });
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { value = "(SENSITIVE)", includeUser });
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> CreateAccessTokenAsync(ulong id, ulong userId, string value)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, userId, value = "(SENSITIVE)" });
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = new AccessTokenEntity(id, userId, value);
            _accessTokenEntities.Add(accessTokenEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> UpdateAccessTokenAsync(AccessToken accessToken)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { accessToken });
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = _accessTokenConverter.ConvertToEntity(accessToken);
            _accessTokenEntities.Update(accessTokenEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InternalServerException("Can't update the accessTokenEntity DbUpdateConcurrencyException!",
                    parameters: accessTokenEntity, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task DeleteAccessTokenAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _accessTokenEntities.Remove(accessTokenEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { accessTokenEntity });
        }

        public async Task<bool> DoesAccessTokenIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesAccessTokenValueExistAsync(string value)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { value = "(SENSITIVE)" });
            var sw = Stopwatch.StartNew();
            var doesExist = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class AccessTokenRepositoryExtensions
    {
        public static IQueryable<AccessTokenEntity> Include(
            [NotNull] this IQueryable<AccessTokenEntity> accessTokenEntitiesQuery,
            bool userEntity)
        {
            if (userEntity)
                accessTokenEntitiesQuery = accessTokenEntitiesQuery.Include(e => e.UserEntity);

            return accessTokenEntitiesQuery;
        }
    }
}

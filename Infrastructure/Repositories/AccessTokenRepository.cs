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
using System.Threading.Tasks;
using FireplaceApi.Core.Models.UserInformations;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces.IRepositories;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly ILogger<AccessTokenRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<AccessTokenEntity> _accessTokenEntities;
        private readonly AccessTokenConverter _accessTokenConverter;

        public AccessTokenRepository(ILogger<AccessTokenRepository> logger, IConfiguration configuration, 
            FireplaceApiContext fireplaceApiContext, AccessTokenConverter accessTokenConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _accessTokenEntities = fireplaceApiContext.AccessTokenEntities;
            _accessTokenConverter = accessTokenConverter;
        }

        public async Task<List<AccessToken>> ListAccessTokensAsync(
                    bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntities = await _accessTokenEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", new { includeUser }, new { accessTokenEntities });
            return accessTokenEntities.Select(e => _accessTokenConverter.ConvertToModel(e)).ToList();
        }

        public async Task<AccessToken> GetAccessTokenByIdAsync(long id, bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { id, includeUser }, new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { value, includeUser }, new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> CreateAccessTokenAsync(long userId, string value)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = new AccessTokenEntity(userId, value);
            _accessTokenEntities.Add(accessTokenEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { userId, value }, new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> UpdateAccessTokenAsync(AccessToken accessToken)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = _accessTokenConverter.ConvertToEntity(accessToken);
            _accessTokenEntities.Update(accessTokenEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the accessTokenEntity DbUpdateConcurrencyException. {accessTokenEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database", new { accessToken }, new { accessTokenEntity });
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task DeleteAccessTokenAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var accessTokenEntity = await _accessTokenEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _accessTokenEntities.Remove(accessTokenEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { id }, new { accessTokenEntity });
        }

        public async Task<bool> DoesAccessTokenIdExistAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { id }, new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesAccessTokenValueExistAsync(string value)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { value }, new { doesExist });
            return doesExist;
        }
    }

    public static class AccessTokenRepositoryExtensions
    {
        public static IQueryable<AccessTokenEntity> Include(
                    [NotNull] this IQueryable<AccessTokenEntity> accessTokenEntitiesQuery,
                    bool userEntity
                    )
        {
            if (userEntity)
                accessTokenEntitiesQuery = accessTokenEntitiesQuery.Include(e => e.UserEntity);

            return accessTokenEntitiesQuery;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Infrastructure.Converters;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Infrastructure.Entities.UserInformationEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Interfaces.IRepositories;

namespace GamingCommunityApi.Infrastructure.Repositories
{
    public class AccessTokenRepository : IAccessTokenRepository
    {
        private readonly ILogger<AccessTokenRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly GamingCommunityApiContext _gamingCommunityApiContext;
        private readonly DbSet<AccessTokenEntity> _accessTokenEntities;
        private readonly AccessTokenConverter _accessTokenConverter;

        public AccessTokenRepository(ILogger<AccessTokenRepository> logger, IConfiguration configuration, 
            GamingCommunityApiContext gamingCommunityApiContext, AccessTokenConverter accessTokenConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _accessTokenEntities = gamingCommunityApiContext.AccessTokenEntities;
            _accessTokenConverter = accessTokenConverter;
        }

        public async Task<List<AccessToken>> ListAccessTokensAsync(
                    bool includeUser = false)
        {
            var accessTokenEntities = await _accessTokenEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            return accessTokenEntities.Select(e => _accessTokenConverter.ConvertToModel(e)).ToList();
        }

        public async Task<AccessToken> GetAccessTokenByIdAsync(long id, bool includeUser = false)
        {
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false)
        {
            var accessTokenEntity = await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> CreateAccessTokenAsync(long userId, string value)
        {
            var accessTokenEntity = new AccessTokenEntity(userId, value);
            _accessTokenEntities.Add(accessTokenEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task<AccessToken> UpdateAccessTokenAsync(AccessToken accessToken)
        {
            var accessTokenEntity = _accessTokenConverter.ConvertToEntity(accessToken);
            _accessTokenEntities.Update(accessTokenEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the accessTokenEntity DbUpdateConcurrencyException. {accessTokenEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _accessTokenConverter.ConvertToModel(accessTokenEntity);
        }

        public async Task DeleteAccessTokenAsync(long id)
        {
            var accessTokenEntity = await _accessTokenEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _accessTokenEntities.Remove(accessTokenEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesAccessTokenIdExistAsync(long id)
        {
            return await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task<bool> DoesAccessTokenValueExistAsync(string value)
        {
            return await _accessTokenEntities
                .AsNoTracking()
                .Where(e => e.Value == value)
                .AnyAsync();
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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models.UserInformations;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces.IRepositories;
using FireplaceApi.Core.ValueObjects;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class GoogleUserRepository : IGoogleUserRepository
    {
        private readonly ILogger<GoogleUserRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<GoogleUserEntity> _googleUserEntities;
        private readonly GoogleUserConverter _googleUserConverter;

        public GoogleUserRepository(ILogger<GoogleUserRepository> logger, IConfiguration configuration, 
            FireplaceApiContext fireplaceApiContext, GoogleUserConverter googleUserConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _googleUserEntities = fireplaceApiContext.GoogleUserEntities;
            _googleUserConverter = googleUserConverter;
        }

        public async Task<List<GoogleUser>> ListGoogleUsersAsync(
                    bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntities = await _googleUserEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", new { includeUser }, new { googleUserEntities });
            return googleUserEntities.Select(e => _googleUserConverter.ConvertToModel(e)).ToList();
        }

        public async Task<GoogleUser> GetGoogleUserByIdAsync(long id, bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { id, includeUser }, new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress, 
            bool includeUser = false)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { gmailAddress, includeUser }, new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> CreateGoogleUserAsync(long userId, string code,
            string accessToken, string tokenType, long accessTokenExpiresInSeconds, 
            string refreshToken, string scope, string idToken, 
            DateTime accessTokenIssuedTime, string gmailAddress, bool gmailVerified,
            long gmailIssuedTimeInSeconds, string fullName, string firstName, 
            string lastName, string locale, string pictureUrl, string state,
            string authUser, string prompt, string redirectToUserUrl)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntity = new GoogleUserEntity(userId, code, accessToken,
                tokenType, accessTokenExpiresInSeconds, refreshToken, scope, idToken,
                accessTokenIssuedTime, gmailAddress, gmailVerified, gmailIssuedTimeInSeconds,
                fullName, firstName, lastName, locale, pictureUrl, state, authUser,
                prompt, redirectToUserUrl);
            _googleUserEntities.Add(googleUserEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();
            
            _logger.LogIOInformation(sw, "Database", 
                new { userId, scope, accessTokenIssuedTime, gmailAddress, fullName, firstName, lastName }, 
                new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> UpdateGoogleUserAsync(GoogleUser googleUser)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntity = _googleUserConverter.ConvertToEntity(googleUser);
            _googleUserEntities.Update(googleUserEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the googleUserEntity DbUpdateConcurrencyException. {googleUserEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database", new { googleUser }, new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task DeleteGoogleUserAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _googleUserEntities.Remove(googleUserEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { id }, new { googleUserEntity });
        }

        public async Task<bool> DoesGoogleUserIdExistAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        
            _logger.LogIOInformation(sw, "Database", new { id }, new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .AnyAsync();
        
            _logger.LogIOInformation(sw, "Database", new { gmailAddress }, new { doesExist });
            return doesExist;
        }
    }

    public static class GoogleUserRepositoryExtensions
    {
        public static IQueryable<GoogleUserEntity> Include(
            [NotNull] this IQueryable<GoogleUserEntity> googleUserEntitiesQuery,
            bool userEntity)
        {
            if (userEntity)
                googleUserEntitiesQuery = googleUserEntitiesQuery.Include(e => e.UserEntity);

            return googleUserEntitiesQuery;
        }

        public static GoogleUserEntity RemoveLoopReferencing([NotNull] this GoogleUserEntity googleUserEntity)
        {
            if (googleUserEntity.UserEntity != null)
                googleUserEntity.UserEntity.GoogleUserEntity = null;

            return googleUserEntity;
        }
    }
}

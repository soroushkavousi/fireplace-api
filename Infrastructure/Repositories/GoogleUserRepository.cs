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
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces.IRepositories;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class GoogleUserRepository : IGoogleUserRepository
    {
        private readonly ILogger<GoogleUserRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _gamingCommunityApiContext;
        private readonly DbSet<GoogleUserEntity> _googleUserEntities;
        private readonly GoogleUserConverter _googleUserConverter;

        public GoogleUserRepository(ILogger<GoogleUserRepository> logger, IConfiguration configuration, 
            FireplaceApiContext gamingCommunityApiContext, GoogleUserConverter googleUserConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _googleUserEntities = gamingCommunityApiContext.GoogleUserEntities;
            _googleUserConverter = googleUserConverter;
        }

        public async Task<List<GoogleUser>> ListGoogleUsersAsync(
                    bool includeUser = false)
        {
            var googleUserEntities = await _googleUserEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            return googleUserEntities.Select(e => _googleUserConverter.ConvertToModel(e)).ToList();
        }

        public async Task<GoogleUser> GetGoogleUserByIdAsync(long id, bool includeUser = false)
        {
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress, 
            bool includeUser = false)
        {
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

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
            var googleUserEntity = new GoogleUserEntity(userId, code, accessToken,
                tokenType, accessTokenExpiresInSeconds, refreshToken, scope, idToken,
                accessTokenIssuedTime, gmailAddress, gmailVerified, gmailIssuedTimeInSeconds,
                fullName, firstName, lastName, locale, pictureUrl, state, authUser,
                prompt, redirectToUserUrl);
            _googleUserEntities.Add(googleUserEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> UpdateGoogleUserAsync(GoogleUser googleUser)
        {
            var googleUserEntity = _googleUserConverter.ConvertToEntity(googleUser);
            _googleUserEntities.Update(googleUserEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the googleUserEntity DbUpdateConcurrencyException. {googleUserEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task DeleteGoogleUserAsync(long id)
        {
            var googleUserEntity = await _googleUserEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _googleUserEntities.Remove(googleUserEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesGoogleUserIdExistAsync(long id)
        {
            return await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress)
        {
            return await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .AnyAsync();
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

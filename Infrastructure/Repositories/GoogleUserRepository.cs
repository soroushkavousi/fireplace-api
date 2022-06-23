using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class GoogleUserRepository : IGoogleUserRepository
    {
        private readonly ILogger<GoogleUserRepository> _logger;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<GoogleUserEntity> _googleUserEntities;
        private readonly GoogleUserConverter _googleUserConverter;

        public GoogleUserRepository(ILogger<GoogleUserRepository> logger,
            FireplaceApiContext fireplaceApiContext, GoogleUserConverter googleUserConverter)
        {
            _logger = logger;
            _fireplaceApiContext = fireplaceApiContext;
            _googleUserEntities = fireplaceApiContext.GoogleUserEntities;
            _googleUserConverter = googleUserConverter;
        }

        public async Task<List<GoogleUser>> ListGoogleUsersAsync(
                    bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { includeUser });
            var sw = Stopwatch.StartNew();
            var googleUserEntities = await _googleUserEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntities });
            return googleUserEntities.Select(e => _googleUserConverter.ConvertToModel(e)).ToList();
        }

        public async Task<GoogleUser> GetGoogleUserByIdAsync(ulong id, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, includeUser });
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress,
            bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { gmailAddress, includeUser });
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> CreateGoogleUserAsync(ulong id, ulong userId,
            string code, string accessToken, string tokenType, long accessTokenExpiresInSeconds,
            string refreshToken, string scope, string idToken,
            DateTime accessTokenIssuedTime, string gmailAddress, bool gmailVerified,
            long gmailIssuedTimeInSeconds, string fullName, string firstName,
            string lastName, string locale, string pictureUrl, string state,
            string authUser, string prompt, string redirectToUserUrl)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new
                {
                    id,
                    userId,
                    code,
                    accessToken,
                    tokenType,
                    accessTokenExpiresInSeconds,
                    refreshToken,
                    scope,
                    idToken,
                    accessTokenIssuedTime,
                    gmailAddress,
                    gmailVerified,
                    gmailIssuedTimeInSeconds,
                    fullName,
                    firstName,
                    lastName,
                    locale,
                    pictureUrl,
                    state,
                    authUser,
                    prompt,
                    redirectToUserUrl
                });
            var sw = Stopwatch.StartNew();
            var googleUserEntity = new GoogleUserEntity(id, userId, code, accessToken,
                tokenType, accessTokenExpiresInSeconds, refreshToken, scope, idToken,
                accessTokenIssuedTime, gmailAddress, gmailVerified, gmailIssuedTimeInSeconds,
                fullName, firstName, lastName, locale, pictureUrl, state, authUser,
                prompt, redirectToUserUrl);
            _googleUserEntities.Add(googleUserEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task<GoogleUser> UpdateGoogleUserAsync(GoogleUser googleUser)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { googleUser });
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

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntity });
            return _googleUserConverter.ConvertToModel(googleUserEntity);
        }

        public async Task DeleteGoogleUserAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var googleUserEntity = await _googleUserEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _googleUserEntities.Remove(googleUserEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { googleUserEntity });
        }

        public async Task<bool> DoesGoogleUserIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { gmailAddress });
            var sw = Stopwatch.StartNew();
            var doesExist = await _googleUserEntities
                .AsNoTracking()
                .Where(e => e.GmailAddress == gmailAddress)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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

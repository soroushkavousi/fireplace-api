using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Tools;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.Interfaces.IGateways;

namespace GamingCommunityApi.Core.Operators
{
    public class GoogleUserOperator
    {
        private readonly ILogger<GoogleUserOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGoogleUserRepository _googleUserRepository;
        private readonly IGoogleGateway _googleUserGateway;

        public GoogleUserOperator(ILogger<GoogleUserOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IGoogleUserRepository googleUserRepository,
            IGoogleGateway googleUserGateway)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _googleUserRepository = googleUserRepository;
            _googleUserGateway = googleUserGateway;
        }

        public async Task<List<GoogleUser>> ListGoogleUsersAsync(bool includeUser = false)
        {
            var googleUser = await _googleUserRepository.ListGoogleUsersAsync(includeUser);
            return googleUser;
        }

        public async Task<GoogleUser> GetGoogleUserByIdAsync(long id, bool includeUser = false)
        {
            var googleUser = await _googleUserRepository.GetGoogleUserByIdAsync(id, includeUser);
            if (googleUser == null)
                return googleUser;

            return googleUser;
        }

        public async Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress,
            bool includeUser = false)
        {
            var googleUser = await _googleUserRepository
                .GetGoogleUserByGmailAddressAsync(gmailAddress, includeUser);
            if (googleUser == null)
                return googleUser;

            return googleUser;
        }

        public async Task<GoogleUser> CreateGoogleUserAsync(long userId, 
            GoogleUserToken googleUserToken, string state,
            string authUser, string prompt, string redirectToUserUrl)
        {
            var googleUser = await _googleUserRepository.CreateGoogleUserAsync(userId,
                googleUserToken.Code, googleUserToken.AccessToken,
                googleUserToken.TokenType, googleUserToken.AccessTokenExpiresInSeconds,
                googleUserToken.RefreshToken, googleUserToken.Scope,
                googleUserToken.IdToken, googleUserToken.AccessTokenIssuedTime,
                googleUserToken.GmailAddress, googleUserToken.GmailVerified,
                googleUserToken.GmailIssuedTimeInSeconds, googleUserToken.FullName,
                googleUserToken.FirstName, googleUserToken.LastName,
                googleUserToken.Locale, googleUserToken.PictureUrl, state,
                authUser, prompt, redirectToUserUrl);
            return googleUser;
        }

        public async Task<string> GetRedirectToUserUrlFromState()
        {
            string redirectToUserUrl;
            redirectToUserUrl = string.Empty;
            await Task.CompletedTask;
            return redirectToUserUrl;
        }

        public async Task<GoogleUser> PatchGoogleUserByIdAsync(long id, long? userId = null, 
            string code = null, string accessToken = null, string tokenType = null,
            long? accessTokenExpiresInSeconds = null, string refreshToken = null, 
            string scope = null, string idToken = null, DateTime? accessTokenIssuedTime = null, 
            string gmailAddress = null, bool? gmailVerified = null, 
            long? gmailIssuedTimeInSeconds = null, string fullName = null, string firstName = null, 
            string lastName = null, string locale = null, string pictureUrl = null)
        {
            var googleUser = await _googleUserRepository.GetGoogleUserByIdAsync(id, true);
            googleUser = await ApplyGoogleUserChangesAsync(googleUser, userId,
                code, accessToken, tokenType, accessTokenExpiresInSeconds, refreshToken,
                scope, idToken, accessTokenIssuedTime, gmailAddress, gmailVerified,
                gmailIssuedTimeInSeconds, fullName, firstName, lastName, locale, pictureUrl);
            googleUser = await GetGoogleUserByIdAsync(googleUser.Id, true);
            return googleUser;
        }

        public async Task<GoogleUser> PatchGoogleUserByGmailAddressAsync(string existingGmailAddress, 
            long? userId = null, string code = null, string accessToken = null, string tokenType = null,
            long? accessTokenExpiresInSeconds = null, string refreshToken = null,
            string scope = null, string idToken = null, DateTime? accessTokenIssuedTime = null,
            string gmailAddress = null, bool? gmailVerified = null,
            long? gmailIssuedTimeInSeconds = null, string fullName = null, string firstName = null,
            string lastName = null, string locale = null, string pictureUrl = null)
        {
            var googleUser = await _googleUserRepository.GetGoogleUserByGmailAddressAsync(existingGmailAddress, true);
            googleUser = await ApplyGoogleUserChangesAsync(googleUser, userId,
                code, accessToken, tokenType, accessTokenExpiresInSeconds, refreshToken,
                scope, idToken, accessTokenIssuedTime, gmailAddress, gmailVerified,
                gmailIssuedTimeInSeconds, fullName, firstName, lastName, locale, pictureUrl);
            googleUser = await GetGoogleUserByIdAsync(googleUser.Id, true);
            return googleUser;
        }

        public async Task DeleteGoogleUserAsync(long id)
        {
            await _googleUserRepository.DeleteGoogleUserAsync(id);
        }

        public async Task<bool> DoesGoogleUserIdExistAsync(long id)
        {
            var googleUserIdExists = await _googleUserRepository.DoesGoogleUserIdExistAsync(id);
            return googleUserIdExists;
        }

        public async Task<GoogleUser> ApplyGoogleUserChangesAsync(GoogleUser googleUser, 
            long? userId, string code, string accessToken, string tokenType,
            long? accessTokenExpiresInSeconds, string refreshToken, string scope, 
            string idToken, DateTime? accessTokenIssuedTime, string gmailAddress, 
            bool? gmailVerified, long? gmailIssuedTimeInSeconds, string fullName, 
            string firstName, string lastName, string locale, string pictureUrl)
        {
            if (userId != null)
                googleUser.UserId = userId.Value;

            if (code != null)
                googleUser.Code = code;

            if (accessToken != null)
                googleUser.AccessToken = accessToken;

            if (tokenType != null)
                googleUser.TokenType = tokenType;

            if (accessTokenExpiresInSeconds != null)
                googleUser.AccessTokenExpiresInSeconds = accessTokenExpiresInSeconds.Value;

            if (refreshToken != null)
                googleUser.RefreshToken = refreshToken;

            if (scope != null)
                googleUser.Scope = scope;

            if (idToken != null)
                googleUser.IdToken = idToken;

            if (accessTokenIssuedTime != null)
                googleUser.AccessTokenIssuedTime = accessTokenIssuedTime.Value;

            if (gmailAddress != null)
                googleUser.GmailAddress = gmailAddress;

            if (gmailVerified != null)
                googleUser.GmailVerified = gmailVerified.Value;

            if (gmailIssuedTimeInSeconds != null)
                googleUser.GmailIssuedTimeInSeconds = gmailIssuedTimeInSeconds.Value;

            if (fullName != null)
                googleUser.FullName = fullName;

            if (firstName != null)
                googleUser.FirstName = firstName;

            if (lastName != null)
                googleUser.LastName = lastName;

            if (locale != null)
                googleUser.Locale = locale;

            if (pictureUrl != null)
                googleUser.PictureUrl = pictureUrl;

            googleUser = await _googleUserRepository.UpdateGoogleUserAsync(googleUser);
            return googleUser;
        }

        public async Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress)
        {
            return await _googleUserRepository
                .DoesGoogleUserGmailAddressExistAsync(gmailAddress);
        }
    }
}

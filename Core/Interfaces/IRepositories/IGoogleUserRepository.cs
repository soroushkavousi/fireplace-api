using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Interfaces
{
    public interface IGoogleUserRepository
    {
        public Task<List<GoogleUser>> ListGoogleUsersAsync(bool includeUser = false);
        public Task<GoogleUser> GetGoogleUserByIdAsync(long id, bool includeUser = false);
        public Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress, 
            bool includeUser = false);
        public Task<GoogleUser> CreateGoogleUserAsync(long userId, string code,
            string accessToken, string tokenType, long accessTokenExpiresInSeconds,
            string refreshToken, string scope, string idToken,
            DateTime accessTokenIssuedTime, string gmailAddress, bool gmailVerified,
            long gmailIssuedTimeInSeconds, string fullName, string firstName,
            string lastName, string locale, string pictureUrl, string state,
            string authUser, string prompt, string redirectToUserUrl);
        public Task<GoogleUser> UpdateGoogleUserAsync(GoogleUser googleUser);
        public Task DeleteGoogleUserAsync(long id);
        public Task<bool> DoesGoogleUserIdExistAsync(long id);
        public Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress);
    }
}

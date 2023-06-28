using FireplaceApi.Domain.GoogleUsers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.GoogleUsers;

public interface IGoogleUserRepository
{
    public Task<List<GoogleUser>> ListGoogleUsersAsync(bool includeUser = false);
    public Task<GoogleUser> GetGoogleUserByIdAsync(ulong id, bool includeUser = false);
    public Task<GoogleUser> GetGoogleUserByGmailAddressAsync(string gmailAddress,
        bool includeUser = false);
    public Task<GoogleUser> CreateGoogleUserAsync(ulong userId, string code,
        string accessToken, string tokenType, long accessTokenExpiresInSeconds,
        string refreshToken, string scope, string idToken,
        DateTime accessTokenIssuedTime, string gmailAddress, bool gmailVerified,
        long gmailIssuedTimeInSeconds, string fullName, string firstName,
        string lastName, string locale, string pictureUrl, string state,
        string authUser, string prompt, string redirectToUserUrl);
    public Task<GoogleUser> UpdateGoogleUserAsync(GoogleUser googleUser);
    public Task DeleteGoogleUserAsync(ulong id);
    public Task<bool> DoesGoogleUserIdExistAsync(ulong id);
    public Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress);
}

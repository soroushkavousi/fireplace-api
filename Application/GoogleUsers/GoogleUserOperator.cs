using FireplaceApi.Domain.GoogleUsers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.GoogleUsers;

public class GoogleUserOperator
{
    private readonly ILogger<GoogleUserOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IGoogleUserRepository _googleUserRepository;
    private readonly IGoogleGateway _googleUserGateway;

    public GoogleUserOperator(ILogger<GoogleUserOperator> logger,
        IServiceProvider serviceProvider, IGoogleUserRepository googleUserRepository,
        IGoogleGateway googleUserGateway)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _googleUserRepository = googleUserRepository;
        _googleUserGateway = googleUserGateway;
    }

    public async Task<List<GoogleUser>> ListGoogleUsersAsync(bool includeUser = false)
    {
        var googleUser = await _googleUserRepository.ListGoogleUsersAsync(includeUser);
        return googleUser;
    }

    public async Task<GoogleUser> GetGoogleUserByIdAsync(ulong id, bool includeUser = false)
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

    public async Task<GoogleUser> CreateGoogleUserAsync(ulong userId,
        GoogleUserToken googleUserToken, string state,
        string authUser, string prompt, string redirectToUserUrl)
    {
        var id = await IdGenerator.GenerateNewIdAsync(DoesGoogleUserIdExistAsync);
        var googleUser = await _googleUserRepository.CreateGoogleUserAsync(id,
            userId, googleUserToken.Code, googleUserToken.AccessToken,
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

    public async Task DeleteGoogleUserAsync(ulong id)
    {
        await _googleUserRepository.DeleteGoogleUserAsync(id);
    }

    public async Task<string> GetRedirectToUserUrlFromState()
    {
        string redirectToUserUrl;
        redirectToUserUrl = string.Empty;
        await Task.CompletedTask;
        return redirectToUserUrl;
    }

    public async Task<GoogleUser> PatchGoogleUserByIdAsync(ulong id,
        ulong? userId = null, GoogleUserToken googleUserToken = null,
        string code = null, string accessToken = null, string tokenType = null,
        long? accessTokenExpiresInSeconds = null, string refreshToken = null, string scope = null,
        string idToken = null, DateTime? accessTokenIssuedTime = null, string gmailAddress = null,
        bool? gmailVerified = null, long? gmailIssuedTimeInSeconds = null, string fullName = null,
        string firstName = null, string lastName = null, string locale = null, string pictureUrl = null,
        string state = null, string authUser = null, string prompt = null, string redirectToUserUrl = null)
    {
        var googleUser = await _googleUserRepository.GetGoogleUserByIdAsync(id, true);
        googleUser = await PatchGoogleUserAsync(googleUser, userId, googleUserToken,
            code, accessToken, tokenType, accessTokenExpiresInSeconds, refreshToken,
            scope, idToken, accessTokenIssuedTime, gmailAddress, gmailVerified,
            gmailIssuedTimeInSeconds, fullName, firstName, lastName, locale, pictureUrl,
            state, authUser, prompt, redirectToUserUrl);
        return googleUser;
    }

    public async Task<GoogleUser> PatchGoogleUserByGmailAddressAsync(string existingGmailAddress,
        ulong? userId = null, GoogleUserToken googleUserToken = null,
        string code = null, string accessToken = null, string tokenType = null,
        long? accessTokenExpiresInSeconds = null, string refreshToken = null, string scope = null,
        string idToken = null, DateTime? accessTokenIssuedTime = null, string gmailAddress = null,
        bool? gmailVerified = null, long? gmailIssuedTimeInSeconds = null, string fullName = null,
        string firstName = null, string lastName = null, string locale = null, string pictureUrl = null,
        string state = null, string authUser = null, string prompt = null, string redirectToUserUrl = null)
    {
        var googleUser = await _googleUserRepository.GetGoogleUserByGmailAddressAsync(existingGmailAddress, true);
        googleUser = await PatchGoogleUserAsync(googleUser, userId, googleUserToken,
            code, accessToken, tokenType, accessTokenExpiresInSeconds, refreshToken,
            scope, idToken, accessTokenIssuedTime, gmailAddress, gmailVerified,
            gmailIssuedTimeInSeconds, fullName, firstName, lastName, locale, pictureUrl,
            state, authUser, prompt, redirectToUserUrl);
        return googleUser;
    }

    public async Task<GoogleUser> PatchGoogleUserAsync(GoogleUser googleUser,
        ulong? userId = null, GoogleUserToken googleUserToken = null,
        string code = null, string accessToken = null, string tokenType = null,
        long? accessTokenExpiresInSeconds = null, string refreshToken = null, string scope = null,
        string idToken = null, DateTime? accessTokenIssuedTime = null, string gmailAddress = null,
        bool? gmailVerified = null, long? gmailIssuedTimeInSeconds = null, string fullName = null,
        string firstName = null, string lastName = null, string locale = null, string pictureUrl = null,
        string state = null, string authUser = null, string prompt = null, string redirectToUserUrl = null)
    {
        if (userId != null)
            googleUser.UserId = userId.Value;

        if (googleUser != null)
        {
            googleUser.Code = googleUserToken.Code;
            googleUser.AccessToken = googleUserToken.AccessToken;
            googleUser.TokenType = googleUserToken.TokenType;
            googleUser.AccessTokenExpiresInSeconds = googleUserToken.AccessTokenExpiresInSeconds;
            googleUser.RefreshToken = googleUserToken.RefreshToken;
            googleUser.Scope = googleUserToken.Scope;
            googleUser.IdToken = googleUserToken.IdToken;
            googleUser.AccessTokenIssuedTime = googleUserToken.AccessTokenIssuedTime;
            googleUser.GmailAddress = googleUserToken.GmailAddress;
            googleUser.GmailVerified = googleUserToken.GmailVerified;
            googleUser.GmailIssuedTimeInSeconds = googleUserToken.GmailIssuedTimeInSeconds;
            googleUser.FullName = googleUserToken.FullName;
            googleUser.FirstName = googleUserToken.FirstName;
            googleUser.LastName = googleUserToken.LastName;
            googleUser.Locale = googleUserToken.Locale;
            googleUser.PictureUrl = googleUserToken.PictureUrl;
        }

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

        if (state != null)
            googleUser.State = state;

        if (authUser != null)
            googleUser.AuthUser = authUser;

        if (prompt != null)
            googleUser.Prompt = prompt;

        if (redirectToUserUrl != null)
            googleUser.RedirectToUserUrl = redirectToUserUrl;

        googleUser = await _googleUserRepository.UpdateGoogleUserAsync(googleUser);
        return googleUser;
    }

    public async Task<bool> DoesGoogleUserIdExistAsync(ulong id)
    {
        var googleUserIdExists = await _googleUserRepository.DoesGoogleUserIdExistAsync(id);
        return googleUserIdExists;
    }

    public async Task<bool> DoesGoogleUserGmailAddressExistAsync(string gmailAddress)
    {
        return await _googleUserRepository
            .DoesGoogleUserGmailAddressExistAsync(gmailAddress);
    }
}

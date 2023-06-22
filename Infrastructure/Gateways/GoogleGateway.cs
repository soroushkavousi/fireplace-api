using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.ValueObjects;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using static Google.Apis.Oauth2.v2.Oauth2Service;

namespace FireplaceApi.Infrastructure.Gateways;

public class GoogleGateway : IGoogleGateway
{
    private readonly ILogger<GoogleGateway> _logger;
    private const string _baseAuthUrl = @"https://accounts.google.com/o/oauth2/auth";
    private const string _baseTokenUrl = @"https://oauth2.googleapis.com/token";

    public GoogleGateway(ILogger<GoogleGateway> logger)
    {
        _logger = logger;
    }

    public async Task<GoogleUserToken> GetGoogleUserToken(string code)
    {
        var googleConfigs = Configs.Current.Google;
        var redirectUrl = Configs.Current.Api.BaseUrlPath
                + googleConfigs.RelativeRedirectUrl;

        return await GetGoogleUserToken(googleConfigs.ClientId,
            googleConfigs.ClientSecret, redirectUrl, code);
    }

    public async Task<GoogleUserToken> GetGoogleUserToken(string clientId,
        string clientSecret, string redirectUrl, string code)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { ScopeConstants.Openid, ScopeConstants.UserinfoProfile, ScopeConstants.UserinfoEmail },
                IncludeGrantedScopes = true,
            });

            var authorizationCodeRequest = flow.CreateAuthorizationCodeRequest(redirectUrl);
            authorizationCodeRequest.State = "swagger";

            var tokenResponse = await flow.ExchangeCodeForTokenAsync("", code,
               redirectUrl, CancellationToken.None);

            var idTokenPayload = await ValidateAsync(tokenResponse.IdToken);
            if (idTokenPayload == null)
                throw new InternalServerException("Google id token is not valid!");

            var googleUser = new GoogleUserToken(code, tokenResponse.AccessToken,
                tokenResponse.TokenType, tokenResponse.ExpiresInSeconds.Value,
                tokenResponse.RefreshToken, tokenResponse.Scope, tokenResponse.IdToken,
                tokenResponse.IssuedUtc, idTokenPayload.Email, idTokenPayload.EmailVerified,
                idTokenPayload.IssuedAtTimeSeconds.Value, idTokenPayload.Name,
                idTokenPayload.GivenName, idTokenPayload.FamilyName,
                idTokenPayload.Locale, idTokenPayload.Picture);

            _logger.LogAppInformation(sw: sw, parameters: new { googleUser });

            return googleUser;
        }
        catch (Exception ex)
        {
            _logger.LogAppError($"Problem", sw, ex: ex);
            throw new InternalServerException("Can't exchange user code for access token",
                parameters: new { code }, systemException: ex);
        }
    }

    public string GetTokenUrl(string code)
    {
        var googleConfigs = Configs.Current.Google;
        var apiConfigs = Configs.Current.Api;
        var redirectUrl = $"{apiConfigs.BaseUrlPath}{googleConfigs.RelativeRedirectUrl}";

        return GetTokenUrl(_baseTokenUrl,
            googleConfigs.ClientId, googleConfigs.ClientSecret,
            code, "authorization_code", redirectUrl);
    }

    public string GetTokenUrl(string baseTokenUrl,
        string clientId, string clientSecret, string code,
        string grantType, string redirectUrl)
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", code },
            { "grant_type", grantType },
            { "redirect_uri", redirectUrl },
        };

        var TokenUrl = QueryHelpers.AddQueryString(baseTokenUrl,
            queryParameters);

        return TokenUrl;

        // With using the libraries
        //var clientSecrets = new ClientSecrets
        //{
        //    ClientId = clientId,
        //    ClientSecret = clientSecret
        //};

        //var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        //{
        //    ClientSecrets = clientSecrets,
        //    Scopes = new[] { ScopeConstants.Openid, ScopeConstants.UserinfoProfile, ScopeConstants.UserinfoEmail },
        //    IncludeGrantedScopes = true,
        //});

        //var authorizationCodeRequest = flow.CreateAuthorizationCodeRequest(redirectUrl);

        //authorizationCodeRequest.State = "swagger";
        //var urlTest = authorizationCodeRequest.Build();
        //var url = urlTest.AbsoluteUri;
    }

    public string GetAuthUrl()
    {
        var googleConfigs = Configs.Current.Google;
        var apiConfigs = Configs.Current.Api;
        var redirectUrl = $"{apiConfigs.BaseUrlPath}{googleConfigs.RelativeRedirectUrl}";

        return GetAuthUrl(_baseAuthUrl,
            googleConfigs.ClientId, redirectUrl, "code",
            "openid profile email", "online", "swagger", "true", "page");
    }

    public string GetAuthUrl(string baseAuthUrl,
        string clientId, string redirectUrl, string responseType,
        string scope, string accessType, string state,
        string includeGrantedScope, string display)
    {
        var queryParameters = new Dictionary<string, string>()
        {
            { "client_id", clientId },
            { "redirect_uri", redirectUrl },
            { "response_type", responseType },
            { "scope", scope },
            { "access_type", accessType },
            { "state", state },
            { "include_granted_scopes", includeGrantedScope },
            { "display", display }
        };

        var authUrl = QueryHelpers.AddQueryString(baseAuthUrl,
            queryParameters);

        return authUrl;
    }
}

using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using static Google.Apis.Oauth2.v2.Oauth2Service;

namespace FireplaceApi.Infrastructure.Gateways
{
    public class GoogleGateway : IGoogleGateway
    {
        private readonly ILogger<GoogleGateway> _logger;
        private readonly IConfiguration _configuration;

        public GoogleGateway(ILogger<GoogleGateway> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<GoogleUserToken> GetgoogleUserToken(string code)
        {
            var googleGlobalValues = GlobalOperator.GlobalValues.Google;
            var redirectUrl = GlobalOperator.GlobalValues.Api.BaseUrlPath
                    + googleGlobalValues.RelativeRedirectUrl;

            return await GetgoogleUserToken(googleGlobalValues.ClientId,
                googleGlobalValues.ClientSecret, redirectUrl, code);
        }

        public async Task<GoogleUserToken> GetgoogleUserToken(string clientId,
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

                authorizationCodeRequest.State = "docs";
                var urlTest = authorizationCodeRequest.Build();
                var url = urlTest.AbsoluteUri;

                var tokenResponse = await flow.ExchangeCodeForTokenAsync("", code,
                   redirectUrl, CancellationToken.None);

                // TODO
                //var validPayload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, new ValidateSettings);

                var jwt = new Jwt(tokenResponse.IdToken);
                var idTokenPayload = jwt.ExtractPayload<Payload>();
                var googleUser = new GoogleUserToken(code, tokenResponse.AccessToken,
                    tokenResponse.TokenType, tokenResponse.ExpiresInSeconds.Value,
                    tokenResponse.RefreshToken, tokenResponse.Scope, tokenResponse.IdToken,
                    tokenResponse.IssuedUtc, idTokenPayload.Email, idTokenPayload.EmailVerified,
                    idTokenPayload.IssuedAtTimeSeconds.Value, idTokenPayload.Name,
                    idTokenPayload.GivenName, idTokenPayload.FamilyName,
                    idTokenPayload.Locale, idTokenPayload.Picture);
                _logger.LogAppInformation(sw, $"googleUser: {googleUser.ToJson()}");

                return googleUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(sw, ex, $"Problem");
                var errorServerMessage = $"Can't exchange user code for access token! User code: {code}";
                throw new ApiException(Core.Enums.ErrorName.INTERNAL_SERVER, errorServerMessage);
            }
        }

        public string GetTokenUrl(string code)
        {
            var googleGlobalValues = GlobalOperator.GlobalValues.Google;
            var apiGlobalValues = GlobalOperator.GlobalValues.Api;
            var redirectUrl = $"{apiGlobalValues.BaseUrlPath}{googleGlobalValues.RelativeRedirectUrl}";

            return GetTokenUrl(googleGlobalValues.BaseTokenUrl,
                googleGlobalValues.ClientId, googleGlobalValues.ClientSecret,
                code, "authorization_code", redirectUrl);
        }

        public string GetTokenUrl(string baseTokenUrl,
            string clientId, string clientSecret, string code,
            string grantType, string redirectUrl)
        {
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

            //authorizationCodeRequest.State = "docs";
            //var urlTest = authorizationCodeRequest.Build();
            //var url = urlTest.AbsoluteUri;

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
        }

        public string GetAuthUrl()
        {
            var googleGlobalValues = GlobalOperator.GlobalValues.Google;
            var apiGlobalValues = GlobalOperator.GlobalValues.Api;
            var redirectUrl = $"{apiGlobalValues.BaseUrlPath}{googleGlobalValues.RelativeRedirectUrl}";

            return GetAuthUrl(googleGlobalValues.BaseAuthUrl,
                googleGlobalValues.ClientId, redirectUrl, "code",
                "openid profile email", "online", "docs", "true", "page");
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
}

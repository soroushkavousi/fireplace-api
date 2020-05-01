using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using GamingCommunityApi.Core.Interfaces.IGateways;
using Google.Apis.Auth.OAuth2;
using static Google.Apis.Oauth2.v2.Oauth2Service;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net.Http;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading;

namespace GamingCommunityApi.Infrastructure.Gateways
{
    public class GoogleGateway : IGoogleGateway
    {
        private readonly ILogger<GoogleGateway> _logger;
        private readonly IConfiguration _configuration;
        private readonly Uri _tokenCallbackUri;

        public GoogleGateway(ILogger<GoogleGateway> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //_tokenCallbackUri = new Uri("https://localhost:5021/v0.1/users/google-token-callback");
            _tokenCallbackUri = new Uri("https://localhost:5021/v0.1/users/sign-up-with-google");
        }

        public async Task RequestToken(string clientId, string clientSecret, string userCode)
        {
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
                    Scopes = new[] { ScopeConstants.UserinfoProfile, ScopeConstants.UserinfoEmail, ScopeConstants.Openid },
                    Nonce = "TestNounce"
                });

                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                var tokenResponse = await flow.ExchangeCodeForTokenAsync("UserTest", userCode,
                    _tokenCallbackUri.AbsoluteUri, cancellationToken);

                _logger.LogInformation($"tokenResponse.AccessToken: {tokenResponse.AccessToken}");
                _logger.LogInformation($"tokenResponse.IdToken: {tokenResponse.IdToken}");
                _logger.LogInformation($"tokenResponse.RefreshToken: {tokenResponse.RefreshToken}");
                _logger.LogInformation($"tokenResponse.Scope: {tokenResponse.Scope}");
                _logger.LogInformation($"tokenResponse.TokenType: {tokenResponse.TokenType}");

                //var xxx = flow.CreateAuthorizationCodeRequest(redirectUri);
                //var xxx = flow.Ex(redirectUri);

                //var client = new HttpClient();

                //var disco = await client.GetDiscoveryDocumentAsync("https://demo.identityserver.io");
                //if (disco.IsError) throw new Exception(disco.Error);

                //await client.RequestTokenAsync()
                //var client = new HttpClient();
                //var zz = new Google.Apis.Auth.OAuth2.Requests.AuthorizationCodeTokenRequest() { };
                //new Google.Apis.Http.HttpClientFactory().CreateHttpClient(new Google.Apis.Http.CreateHttpClientArgs { })






                //var z = new Google.Apis.Auth.OAuth2.Requests.AuthorizationCodeTokenRequest() { }

                //var y = new Google.Apis.Oauth2.v2.Oauth2Service() { }
                //var x = new Google.Apis.Oauth2.v2.Oauth2Service.TokeninfoRequest { }
                ////GoogleWebAuthorizationBroker.
                //var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    new ClientSecrets
                //    {
                //        ClientId = clientId,
                //        ClientSecret = clientSecret
                //    },
                //    new[] { ServiceAccountCredential.Scope },
                //    "user",
                //    CancellationToken.None,
                //    new FileDataStore("Books.ListMyLibrary"));

                //Scope.UserinfoProfile
                //GoogleTokenResponse tokenResponse =
                //  new GoogleAuthorizationCodeTokenRequest(
                //      new NetHttpTransport(),
                //      JacksonFactory.getDefaultInstance(),
                //      "https://oauth2.googleapis.com/token",
                //      clientSecrets.getDetails().getClientId(),
                //      clientSecrets.getDetails().getClientSecret(),
                //      authCode,
                //      REDIRECT_URI)  // Specify the same redirect URI that you use with your web
                //                     // app. If you don't have a web version of your app, you can
                //                     // specify an empty string.
                //      .execute();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Problem");
            }
        }
    }
}

using FireplaceApi.Core.Interfaces.IRepositories;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Api.IntegrationTests.Tools
{
    public class ClientPool
    {
        private readonly ILogger<ClientPool> _logger;
        private readonly WebApplicationFactory<Startup> _apiFactory;
        private readonly WebApplicationFactoryClientOptions _clientOptions;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;
        private readonly AccessTokenOperator _accessTokenOperator;

        public HttpClient GuestClient { get; }
        public HttpClient TheHulkClient { get; }

        public ClientPool(ApiIntegrationTestFixture testFixture)
        {
            _logger = testFixture.ServiceProvider.GetRequiredService<ILogger<ClientPool>>();
            _apiFactory = testFixture.ApiFactory;
            _clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost:5021"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };
            _fireplaceApiContext = testFixture.ServiceProvider.GetRequiredService<FireplaceApiContext>();
            _userRepository = testFixture.ServiceProvider.GetRequiredService<IUserRepository>();
            _emailRepository = testFixture.ServiceProvider.GetRequiredService<IEmailRepository>();
            _accessTokenRepository = testFixture.ServiceProvider.GetRequiredService<IAccessTokenRepository>();
            _accessTokenOperator = testFixture.ServiceProvider.GetRequiredService<AccessTokenOperator>();
            GuestClient = CreateGuestClient();
            TheHulkClient = CreateTheHulkClientAsync().GetAwaiter().GetResult();
        }

        private HttpClient CreateGuestClient()
        {
            var guestClient = _apiFactory.CreateClient(_clientOptions);
            var defaultRequestHeaders = guestClient.DefaultRequestHeaders;
            defaultRequestHeaders.Add(Api.Tools.Constants.X_FORWARDED_FOR, @"::1");
            _logger.LogInformation($"Guest client initialized successfully.");
            return guestClient;
        }

        private async Task<HttpClient> CreateTheHulkClientAsync()
        {
            var user = await _userRepository.CreateUserAsync("Bruce", "Banner",
                "TheHulk", Core.Enums.UserState.NOT_VERIFIED, Password.OfValue("TheHulkP0"));
            var emailActivation = new Activation(Core.Enums.ActivationStatus.SENT, 12345, "Code: 12345");
            var email = await _emailRepository.CreateEmailAsync(user.Id, "TheHulk", emailActivation);
            var newAccessTokenValue = _accessTokenOperator.GenerateNewAccessTokenValue();
            var accessToken = await _accessTokenRepository.CreateAccessTokenAsync(user.Id, newAccessTokenValue);
            var theHulkClient = _apiFactory.CreateClient(_clientOptions);
            var defaultRequestHeaders = theHulkClient.DefaultRequestHeaders;
            defaultRequestHeaders.Add(Api.Tools.Constants.AuthorizationHeaderKey, $"Bearer {newAccessTokenValue}");
            defaultRequestHeaders.Add(Api.Tools.Constants.X_FORWARDED_FOR, @"::1");
            _logger.LogInformation($"The Hulk client initialized successfully.");
            return theHulkClient;
        }
    }
}

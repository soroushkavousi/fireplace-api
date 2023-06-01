using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.IntegrationTests.Extensions;
using FireplaceApi.IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireplaceApi.IntegrationTests.Tools;

public class ClientPool
{
    private readonly ILogger<ClientPool> _logger;
    private readonly WebApplicationFactory<Program> _apiFactory;
    private readonly WebApplicationFactoryClientOptions _clientOptions;
    private readonly ProjectDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly AccessTokenOperator _accessTokenOperator;

    public User NarutoUser { get; set; }
    public HttpClient NarutoHttpClient { get; set; }

    public ClientPool(ApiIntegrationTestFixture testFixture)
    {
        _logger = testFixture.ServiceProvider.GetRequiredService<ILogger<ClientPool>>();
        _apiFactory = testFixture.ApiFactory;
        _clientOptions = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri(Configs.Current.Api.BaseUrlPath),
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        };
        _dbContext = testFixture.ServiceProvider.GetRequiredService<ProjectDbContext>();
        _userRepository = testFixture.ServiceProvider.GetRequiredService<IUserRepository>();
        _emailRepository = testFixture.ServiceProvider.GetRequiredService<IEmailRepository>();
        _accessTokenRepository = testFixture.ServiceProvider.GetRequiredService<IAccessTokenRepository>();
        _accessTokenOperator = testFixture.ServiceProvider.GetRequiredService<AccessTokenOperator>();
    }

    public TestGuest CreateGuest()
        => new(CreateHttpClient());

    public async Task<TestUser> CreateNotVerifiedUserAsync()
        => await CreateTestUser(
            username: "NotVerifiedUser",
            displayName: "NotVerified User",
            passwordValue: "NotVerifiedUserPassword111!",
            activationCode: 11111,
            emailAddress: "NotVerifiedUser@gmail.com",
            state: UserState.NOT_VERIFIED
            );

    public async Task<TestUser> CreateNarutoUserAsync()
        => await CreateTestUser(
            username: "Naruto",
            displayName: "Naruto Uzumaki",
            passwordValue: "NarutoPassword222@",
            activationCode: 22222,
            emailAddress: "Naruto@gmail.com",
            state: UserState.VERIFIED
            );

    public async Task<TestUser> CreateSasukeUserAsync()
        => await CreateTestUser(
            username: "Sasuke",
            displayName: "Sasuke Uchiha",
            passwordValue: "SasukePassword333#",
            activationCode: 33333,
            emailAddress: "Sasuke@gmail.com",
            state: UserState.VERIFIED
            );

    private async Task<TestUser> CreateTestUser(string username,
        string displayName, string passwordValue, int activationCode,
        string emailAddress, UserState state)
    {
        var sw = Stopwatch.StartNew();
        var id = await IdGenerator.GenerateNewIdAsync();
        var password = Password.OfValue(passwordValue);
        var user = await _userRepository.CreateUserAsync(id, username,
            state, password, displayName: displayName);
        user.Password = password;

        var activationStatus = state == UserState.VERIFIED ? ActivationStatus.COMPLETED : ActivationStatus.SENT;
        var emailActivation = new Activation(activationStatus, activationCode, "Fireplace Email Activation", $"Code: {activationCode}");
        id = await IdGenerator.GenerateNewIdAsync();
        user.Email = await _emailRepository.CreateEmailAsync(id, user.Id, emailAddress, emailActivation);

        var httpClient = CreateHttpClient();
        var testUser = new TestUser(user, httpClient);
        await LogInUser(testUser);

        _logger.LogAppInformation($"User {username} created successfully.", sw);
        return testUser;
    }

    private async Task LogInUser(TestUser testUser)
    {
        var httpMethod = new HttpMethod("POST");
        var requestUri = "/users/log-in-with-email";
        var request = new HttpRequestMessage(httpMethod, requestUri)
        {
            Content = new
            {
                EmailAddress = testUser.Email.Address,
                Password = testUser.Password.Value
            }.ToHttpContent(),
        };
        var response = await testUser.SendRequestAsync(request);
        testUser.HttpClient.AddOrUpdateAccessToken(response);
    }

    private HttpClient CreateHttpClient()
    {
        var httpClient = _apiFactory.CreateClient(_clientOptions);
        var defaultRequestHeaders = httpClient.DefaultRequestHeaders;
        defaultRequestHeaders.Add(Application.Tools.Constants.X_FORWARDED_FOR, @"::1");
        return httpClient;
    }
}

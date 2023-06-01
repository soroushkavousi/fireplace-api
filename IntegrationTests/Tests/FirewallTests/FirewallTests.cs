using FireplaceApi.Application.Enums;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.IntegrationTests.Extensions;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.FirewallTests;

[Collection("Firewall")]
public class FirewallTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<FirewallTests> _logger;
    private readonly ClientPool _clientPool;

    public FirewallTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<FirewallTests>>();
        _clientPool = _fixture.ClientPool;
    }

    [Fact]
    public async Task GetUser_WhenUserIsGuest_ShouldReturnDeniedAccess()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given

            //When
            var request = new HttpRequestMessage(HttpMethod.Get, "/users/me");
            var testGuest = _clientPool.CreateGuest();
            var response = await testGuest.HttpClient.SendAsync(request);

            //Then
            await response.EnsureResponseIsTheErrorAsync(ErrorType.AUTHENTICATION_FAILED, FieldName.ACCESS_TOKEN);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }

    [Fact]
    public async Task GetUser_WhenUserIsAuthenticated_ShouldReturnAcceptedAccess()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var request = new HttpRequestMessage(HttpMethod.Get, "/users/me");
            var narutoUser = await _clientPool.CreateNarutoUserAsync();

            //When
            var response = await narutoUser.SendRequestAsync(request);

            //Then
            await response.EnsureResponseIsNotTheErrorAsync(ErrorType.AUTHENTICATION_FAILED, FieldName.ACCESS_TOKEN);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }

    [Fact]
    public async Task CreateCommunity_WhenSendingEmptyBody_ShouldReturnInvalidContentType()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();

            //When
            var request = new HttpRequestMessage(HttpMethod.Post, "/communities");
            var response = await narutoUser.SendRequestAsync(request);

            //Then
            await response.EnsureResponseIsTheErrorAsync(ErrorType.INCORRECT_VALUE,
                ApplicationFieldName.REQUEST_CONTENT_TYPE);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }

    [Theory]
    [InlineData("{name:\"test-community-name\"}")]
    public async Task CreateCommunity_WhenSendingBodyWithWrongJsonFormat_ShouldReturnBodyIsNotJsonError(
        string jsonWithWrongFormat)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START", parameters: new { jsonWithWrongFormat });

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();

            //When
            var request = new HttpRequestMessage(HttpMethod.Post, "/communities")
            {
                Content = jsonWithWrongFormat.ToHttpContent(),
            };
            var response = await narutoUser.SendRequestAsync(request);

            //Then
            await response.EnsureResponseIsTheErrorAsync(ErrorType.INVALID_FORMAT,
                ApplicationFieldName.REQUEST_BODY);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

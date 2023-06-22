using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Operators;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommunityTests;

[Collection("Community")]
public class CommunityCreateTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<CommunityCreateTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommunityOperator _communityOperator;

    public CommunityCreateTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommunityCreateTests>>();
        _clientPool = _fixture.ClientPool;
        _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
    }

    [Fact]
    public async Task CreateCommunity_WhenCreatingACommunity_ShouldReturnCreatedCommunity()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();
            var communityName = "test-community-name";

            //When
            var createdCommunity = await CommunityUtils.CreateCommunityWithApiAsync(narutoUser, communityName);

            //Then
            var retrievedCommunity = await CommunityUtils.GetCommunityWithApiAsync(narutoUser, communityName);
            Assert.Equal(createdCommunity.Id, retrievedCommunity.Id);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

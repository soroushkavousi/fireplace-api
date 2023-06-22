using FireplaceApi.Application.Communities;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommunityTests;

[Collection("Community")]
public class CommunityListTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<CommunityListTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommunityOperator _communityOperator;

    public CommunityListTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommunityListTests>>();
        _clientPool = _fixture.ClientPool;
        _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
    }

    [Fact]
    public async Task ListCommunities_WhenSearchingCommunitiesByName_ShouldReturnFilteredCommunities()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();
            var backendDevelopersCommunity = await _communityOperator.CreateCommunityAsync(narutoUser.Id, "backend-developers");
            var gamersCommunity = await _communityOperator.CreateCommunityAsync(narutoUser.Id, "gamers");

            //When
            var communityQueryResult = await CommunityUtils.ListCommunitiesWithApiAsync(narutoUser, "dev");

            //Then
            Assert.Single(communityQueryResult.Items);
            Assert.Equal(backendDevelopersCommunity.Name, communityQueryResult.Items[0].Name);

            _logger.LogAppInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Communities;
using FireplaceApi.IntegrationTests.Tools;
using MediatR;
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
    private readonly ISender _sender;

    public CommunityListTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommunityListTests>>();
        _clientPool = _fixture.ClientPool;
        _sender = _fixture.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task ListCommunities_WhenSearchingCommunitiesByName_ShouldReturnFilteredCommunities()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogServerInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();

            var createCommunityCommand = new CreateCommunityCommand(narutoUser.Id,
                new CommunityName("backend-developers"), narutoUser.Username);
            var backendDevelopersCommunity = await _sender.Send(createCommunityCommand);

            createCommunityCommand = new CreateCommunityCommand(narutoUser.Id,
                new CommunityName("gamers"), narutoUser.Username);
            var gamersCommunity = await _sender.Send(createCommunityCommand);

            //When
            var communityQueryResult = await CommunityUtils.SearchCommunitiesWithApiAsync(narutoUser, "dev");

            //Then
            Assert.Single(communityQueryResult.Items);
            Assert.Equal(backendDevelopersCommunity.Name.Value, communityQueryResult.Items[0].Name);

            _logger.LogServerInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogServerCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

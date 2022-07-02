using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.IntegrationTests.Models;
using FireplaceApi.Api.IntegrationTests.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Api.IntegrationTests
{
    public class CommunityTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _fixture;
        private readonly ILogger<CommunityTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public CommunityTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommunityTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
        }

        [Fact]
        public async Task User_CreateCommunity_CreatedCommunity()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var communityName = "test-community-name";
                var createdCommunity = await CreateCommunity(narutoUser, communityName);
                var retrievedCommunity = await GetCommunity(narutoUser, communityName);
                Assert.Equal(createdCommunity.Id, retrievedCommunity.Id);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        [Fact]
        public async Task User_ListCommunitiesByName_FilteredCommunity()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var backendDevelopersCommunity = await CreateCommunity(narutoUser, "backend-developers");
                var gamersCommunity = await CreateCommunity(narutoUser, "gamers");
                var communityPages = await ListCommunities(narutoUser, "dev");
                Assert.Single(communityPages.Items);
                Assert.Equal(backendDevelopersCommunity.Name, communityPages.Items[0].Name);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        private async Task<CommunityDto> CreateCommunity(TestUser user, string communityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/communities")
            {
                Content = _testUtils.MakeRequestContent(new
                {
                    name = communityName
                })
            };
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdCommunity = responseBody.FromJson<CommunityDto>();
            Assert.NotNull(createdCommunity);
            return createdCommunity;
        }

        private async Task<CommunityDto> GetCommunity(TestUser user, string communityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/communities/{communityName}");
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var retrievedCommunity = responseBody.FromJson<CommunityDto>();
            Assert.NotNull(retrievedCommunity);
            return retrievedCommunity;
        }

        private async Task<PageDto<CommunityDto>> ListCommunities(TestUser user, string communityName)
        {
            var baseUrl = $"/communities";
            var queryParameters = new Dictionary<string, string>()
            {
                { "name", communityName },
            };
            var requestUrl = QueryHelpers.AddQueryString(baseUrl, queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var communityPages = responseBody.FromJson<PageDto<CommunityDto>>();
            return communityPages;
        }
    }
}

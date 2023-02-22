using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.IntegrationTests.Models;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Application.IntegrationTests
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
                var createdCommunity = await CreateCommunityAsync(_testUtils, narutoUser, communityName);
                var retrievedCommunity = await GetCommunityAsync(narutoUser, communityName);
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
                var backendDevelopersCommunity = await CreateCommunityAsync(_testUtils, narutoUser, "backend-developers");
                var gamersCommunity = await CreateCommunityAsync(_testUtils, narutoUser, "gamers");
                var communityQueryResult = await ListCommunitiesAsync(narutoUser, "dev");
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

        public static async Task<CommunityDto> CreateCommunityAsync(TestUtils testUtils, TestUser user, string communityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/communities")
            {
                Content = testUtils.MakeRequestContent(new
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

        public static async Task<CommunityDto> GetCommunityAsync(TestUser user, string communityName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/communities/{communityName}");
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var retrievedCommunity = responseBody.FromJson<CommunityDto>();
            Assert.NotNull(retrievedCommunity);
            return retrievedCommunity;
        }

        public static async Task<QueryResultDto<CommunityDto>> ListCommunitiesAsync(TestUser user, string communityName)
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
            var queryResult = responseBody.FromJson<QueryResultDto<CommunityDto>>();
            return queryResult;
        }
    }
}

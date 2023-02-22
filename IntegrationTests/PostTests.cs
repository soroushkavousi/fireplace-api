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
    public class PostTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _fixture;
        private readonly ILogger<PostTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public PostTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<PostTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
        }

        [Fact]
        public async Task User_CreatePost_CreatedPost()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var animeCommunityName = "anime-community";
                var animeCommunity = await CommunityTests.CreateCommunityAsync(_testUtils,
                    narutoUser, animeCommunityName);
                var postContent = "Sample Post Content";
                var createdPost = await CreatePostAsync(_testUtils, narutoUser, animeCommunityName, postContent);
                var retrievedPost = await GetPostAsync(narutoUser, createdPost.Id);
                Assert.Equal(createdPost.Id, retrievedPost.Id);
                var animePosts = await ListPostsAsync(narutoUser, animeCommunityName);
                Assert.Single(animePosts.Items);
                Assert.Equal(createdPost.Id, animePosts.Items[0].Id);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        public static async Task<PostDto> CreatePostAsync(TestUtils testUtils, TestUser user,
            string communityEncodedIdOrName, string postContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"/communities/{communityEncodedIdOrName}/posts")
            {
                Content = testUtils.MakeRequestContent(new
                {
                    content = postContent
                })
            };
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdPost = responseBody.FromJson<PostDto>();
            Assert.NotNull(createdPost);
            return createdPost;
        }

        public static async Task<PostDto> GetPostAsync(TestUser user, string postEncodedId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/posts/{postEncodedId}");
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var retrievedPost = responseBody.FromJson<PostDto>();
            Assert.NotNull(retrievedPost);
            return retrievedPost;
        }

        public static async Task<QueryResultDto<PostDto>> ListPostsAsync(TestUser user, string communityEncodedIdOrName)
        {
            var baseUrl = $"/communities/{communityEncodedIdOrName}/posts";
            var queryParameters = new Dictionary<string, string>()
            {
                { "sort", "TOP" },
            };
            var requestUrl = QueryHelpers.AddQueryString(baseUrl, queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var queryResult = responseBody.FromJson<QueryResultDto<PostDto>>();
            return queryResult;
        }
    }
}

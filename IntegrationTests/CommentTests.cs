using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.IntegrationTests.Models;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Application.IntegrationTests
{
    public class CommentTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _fixture;
        private readonly ILogger<CommentTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public CommentTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
        }

        [Fact]
        public async Task User_ListManyComments_CorrectQueryResult()
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
                var createdPost = await PostTests.CreatePostAsync(_testUtils, narutoUser, animeCommunityName, postContent);
                var comments = new List<CommentDto>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 4; i++)
                {
                    var comment = await ReplyToPostAsync(narutoUser, createdPost.Id, $"comment {i}");
                    comments.Add(comment);
                }

                var comment10 = comments[9];
                comment10 = await VoteCommentAsync(_testUtils, narutoUser, comment10.Id);

                var comment10Childs = new List<CommentDto>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 6; i++)
                {
                    var comment = await ReplyToCommentAsync(_testUtils, narutoUser, comment10.Id, $"comment 10, {i}");
                    comment10Childs.Add(comment);
                }

                var postComments = await ListPostCommentsAsync(narutoUser, createdPost.Id, SortType.TOP);
                Assert.Equal(Configs.Current.QueryResult.ViewLimit, postComments.Items.Count);
                Assert.Equal(comments.Count - Configs.Current.QueryResult.ViewLimit, postComments.MoreItemIds.Count);

                comment10 = postComments.Items.Single(c => c.Id == comment10.Id);
                Assert.Equal(Configs.Current.QueryResult.ViewLimit, comment10.ChildComments.Count);
                Assert.Equal(comment10Childs.Count - Configs.Current.QueryResult.ViewLimit, comment10.MoreChildCommentIds.Count);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        [Fact]
        public async Task User_CreateNestedComments_CorrectNestedComments()
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
                var createdPost = await PostTests.CreatePostAsync(_testUtils, narutoUser, animeCommunityName, postContent);
                var comment1 = await ReplyToPostAsync(narutoUser, createdPost.Id, "comment 1");
                var comment2 = await ReplyToPostAsync(narutoUser, createdPost.Id, "comment 2");
                var comment3 = await ReplyToPostAsync(narutoUser, createdPost.Id, "comment 3");

                var comment11 = await ReplyToCommentAsync(_testUtils, narutoUser, comment1.Id, "comment 1, 1");
                var comment12 = await ReplyToCommentAsync(_testUtils, narutoUser, comment1.Id, "comment 1, 2");
                var comment13 = await ReplyToCommentAsync(_testUtils, narutoUser, comment1.Id, "comment 1, 3");
                var comment14 = await ReplyToCommentAsync(_testUtils, narutoUser, comment1.Id, "comment 1, 4");

                var comment21 = await ReplyToCommentAsync(_testUtils, narutoUser, comment2.Id, "comment 2, 1");
                var comment22 = await ReplyToCommentAsync(_testUtils, narutoUser, comment2.Id, "comment 2, 2");

                var comment31 = await ReplyToCommentAsync(_testUtils, narutoUser, comment3.Id, "comment 3, 1");
                var comment32 = await ReplyToCommentAsync(_testUtils, narutoUser, comment3.Id, "comment 3, 2");
                var comment33 = await ReplyToCommentAsync(_testUtils, narutoUser, comment3.Id, "comment 3, 3");

                var comment131 = await ReplyToCommentAsync(_testUtils, narutoUser, comment13.Id, "comment 1, 3, 1");
                var comment132 = await ReplyToCommentAsync(_testUtils, narutoUser, comment13.Id, "comment 1, 3, 2");
                var comment133 = await ReplyToCommentAsync(_testUtils, narutoUser, comment13.Id, "comment 1, 3, 3");

                var comment311 = await ReplyToCommentAsync(_testUtils, narutoUser, comment31.Id, "comment 3, 1, 1");
                var comment312 = await ReplyToCommentAsync(_testUtils, narutoUser, comment31.Id, "comment 3, 1, 2");

                var nestedComments = await ListPostCommentsAsync(narutoUser, createdPost.Id, SortType.TOP);
                Assert.Equal(3, nestedComments.Items.Count);
                var receivedComment1 = nestedComments.Items.Single(c => c.Id == comment1.Id);
                var receivedComment2 = nestedComments.Items.Single(c => c.Id == comment2.Id);
                var receivedComment3 = nestedComments.Items.Single(c => c.Id == comment3.Id);
                Assert.Equal(4, receivedComment1.ChildComments.Count);
                Assert.Equal(2, receivedComment2.ChildComments.Count);
                Assert.Equal(3, receivedComment3.ChildComments.Count);
                var receivedComment13 = receivedComment1.ChildComments.Single(c => c.Id == comment13.Id);
                var receivedComment31 = receivedComment3.ChildComments.Single(c => c.Id == comment31.Id);
                Assert.Equal(3, receivedComment13.ChildComments.Count);
                Assert.Equal(2, receivedComment31.ChildComments.Count);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        private async Task<CommentDto> ReplyToPostAsync(TestUser user, string postEncodedId, string content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"/posts/{postEncodedId}/comments")
            {
                Content = _testUtils.MakeRequestContent(new
                {
                    content
                })
            };
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdComment = responseBody.FromJson<CommentDto>();
            Assert.NotNull(createdComment);
            return createdComment;
        }

        public static async Task<CommentDto> ReplyToCommentAsync(TestUtils testUtils, TestUser user,
            string commentEncodedId, string content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"/comments/{commentEncodedId}/comments")
            {
                Content = testUtils.MakeRequestContent(new
                {
                    content
                })
            };
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdComment = responseBody.FromJson<CommentDto>();
            Assert.NotNull(createdComment);
            return createdComment;
        }

        public static async Task<CommentDto> GetCommentAsync(TestUser user, string commentEncodedId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/comments/{commentEncodedId}");
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var retrievedComment = responseBody.FromJson<CommentDto>();
            Assert.NotNull(retrievedComment);
            return retrievedComment;
        }

        public static async Task<QueryResultDto<CommentDto>> ListPostCommentsAsync(TestUser user, string postEncodedId, SortType sort)
        {
            var baseUrl = $"/posts/{postEncodedId}/comments";
            var queryParameters = new Dictionary<string, string>()
            {
                { "sort", sort.ToString() },
            };
            var requestUrl = QueryHelpers.AddQueryString(baseUrl, queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var queryResult = responseBody.FromJson<QueryResultDto<CommentDto>>();
            return queryResult;
        }

        public static async Task<CommentDto> VoteCommentAsync(TestUtils testUtils, TestUser user, string commentEncodedId)
        {
            var url = $"/comments/{commentEncodedId}/votes";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = testUtils.MakeRequestContent(new
                {
                    isUpvote = true,
                })
            };
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var comment = responseBody.FromJson<CommentDto>();
            return comment;
        }
    }
}

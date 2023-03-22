using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.IntegrationTests.Models;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
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
        private readonly CommentOperator _commentOperator;
        private readonly PostOperator _postOperator;
        private readonly CommunityOperator _communityOperator;

        public CommentTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
            _commentOperator = _fixture.ServiceProvider.GetRequiredService<CommentOperator>();
            _postOperator = _fixture.ServiceProvider.GetRequiredService<PostOperator>();
            _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
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
                var animeCommunity = await _communityOperator.CreateCommunityAsync(narutoUser, animeCommunityName);
                var postContent = "Sample Post Content";
                var post = await _postOperator.CreatePostAsync(narutoUser, animeCommunity.Id,
                    animeCommunityName, postContent);
                var comments = new List<Comment>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 4; i++)
                {
                    var comment = await _commentOperator.ReplyToPostAsync(narutoUser, post.Id, $"comment {i}");
                    comments.Add(comment);
                }

                var comment10 = comments[9];
                var comment10Vote = await _commentOperator.VoteCommentAsync(narutoUser, comment10.Id, isUp: true);

                var comment10Childs = new List<Comment>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 6; i++)
                {
                    var comment = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment10.Id, $"comment 10, {i}");
                    comment10Childs.Add(comment);
                }

                var postComments = await ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), SortType.TOP);
                Assert.Equal(Configs.Current.QueryResult.ViewLimit, postComments.Items.Count);
                Assert.Equal(comments.Count - Configs.Current.QueryResult.ViewLimit, postComments.MoreItemIds.Count);

                var comment10Dto = postComments.Items[0];
                Assert.Equal(comment10.Id, comment10Dto.Id.IdDecode());
                Assert.Equal(Configs.Current.QueryResult.ViewLimit, comment10Dto.ChildComments.Count);
                Assert.Equal(comment10Childs.Count - Configs.Current.QueryResult.ViewLimit, comment10Dto.MoreChildCommentIds.Count);

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
                var animeCommunity = await _communityOperator.CreateCommunityAsync(narutoUser, animeCommunityName);
                var postContent = "Sample Post Content";
                var post = await _postOperator.CreatePostAsync(narutoUser,
                    animeCommunity.Id, animeCommunity.Name, postContent);
                var comment1 = await _commentOperator.ReplyToPostAsync(narutoUser, post.Id, "comment 1");
                var comment2 = await _commentOperator.ReplyToPostAsync(narutoUser, post.Id, "comment 2");
                var comment3 = await _commentOperator.ReplyToPostAsync(narutoUser, post.Id, "comment 3");

                var comment11 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment1.Id, "comment 1, 1");
                var comment12 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment1.Id, "comment 1, 2");
                var comment13 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment1.Id, "comment 1, 3");
                var comment14 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment1.Id, "comment 1, 4");

                var comment21 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment2.Id, "comment 2, 1");
                var comment22 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment2.Id, "comment 2, 2");

                var comment31 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment3.Id, "comment 3, 1");
                var comment32 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment3.Id, "comment 3, 2");
                var comment33 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment3.Id, "comment 3, 3");

                var comment131 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment13.Id, "comment 1, 3, 1");
                var comment132 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment13.Id, "comment 1, 3, 2");
                var comment133 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment13.Id, "comment 1, 3, 3");

                var comment311 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment31.Id, "comment 3, 1, 1");
                var comment312 = await _commentOperator.ReplyToCommentAsync(narutoUser, post.Id, comment31.Id, "comment 3, 1, 2");

                var nestedComments = await ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), SortType.TOP);
                Assert.Equal(3, nestedComments.Items.Count);
                var receivedComment1 = nestedComments.Items.Single(c => c.Id.IdDecode() == comment1.Id);
                var receivedComment2 = nestedComments.Items.Single(c => c.Id.IdDecode() == comment2.Id);
                var receivedComment3 = nestedComments.Items.Single(c => c.Id.IdDecode() == comment3.Id);
                Assert.Equal(4, receivedComment1.ChildComments.Count);
                Assert.Equal(2, receivedComment2.ChildComments.Count);
                Assert.Equal(3, receivedComment3.ChildComments.Count);
                var receivedComment13 = receivedComment1.ChildComments.Single(c => c.Id.IdDecode() == comment13.Id);
                var receivedComment31 = receivedComment3.ChildComments.Single(c => c.Id.IdDecode() == comment31.Id);
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

        private async Task<CommentDto> ReplyToPostWithApiAsync(TestUser user, string postEncodedId, string content)
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

        public static async Task<CommentDto> ReplyToCommentWithApiAsync(TestUtils testUtils, TestUser user,
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

        public static async Task<CommentDto> GetCommentWithApiAsync(TestUser user, string commentEncodedId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/comments/{commentEncodedId}");
            var response = await user.SendRequestAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var retrievedComment = responseBody.FromJson<CommentDto>();
            Assert.NotNull(retrievedComment);
            return retrievedComment;
        }

        public static async Task<QueryResultDto<CommentDto>> ListPostCommentsWithApiAsync(TestUser user, string postEncodedId, SortType sort)
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

        public static async Task<CommentDto> VoteCommentWithApiAsync(TestUtils testUtils, TestUser user, string commentEncodedId)
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

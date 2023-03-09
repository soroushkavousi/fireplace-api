using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.IntegrationTests.Models;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
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
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentVoteRepository _commentVoteRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommunityRepository _communityRepository;

        public CommentTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
            _commentRepository = _fixture.ServiceProvider.GetRequiredService<ICommentRepository>();
            _commentVoteRepository = _fixture.ServiceProvider.GetRequiredService<ICommentVoteRepository>();
            _postRepository = _fixture.ServiceProvider.GetRequiredService<IPostRepository>();
            _communityRepository = _fixture.ServiceProvider.GetRequiredService<ICommunityRepository>();
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
                var animeCommunity = await CommunityTests.CreateCommunityWithRepositoryAsync(_communityRepository,
                    narutoUser, animeCommunityName);
                var postContent = "Sample Post Content";
                var post = await PostTests.CreatePostWithRepositoryAsync(_postRepository, narutoUser, animeCommunity.Id,
                    animeCommunityName, postContent);
                var comments = new List<Comment>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 4; i++)
                {
                    var comment = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, $"comment {i}");
                    comments.Add(comment);
                }

                var comment10 = comments[9];
                var comment10Vote = await VoteCommentWithRepositoryAsync(_commentVoteRepository, narutoUser, comment10.Id, isUp: true);

                var comment10Childs = new List<Comment>();
                for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 6; i++)
                {
                    var comment = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, $"comment 10, {i}", comment10.Id);
                    comment10Childs.Add(comment);
                }

                var postComments = await ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), SortType.TOP);
                Assert.Equal(Configs.Current.QueryResult.ViewLimit, postComments.Items.Count);
                Assert.Equal(comments.Count - Configs.Current.QueryResult.ViewLimit, postComments.MoreItemIds.Count);

                var comment10Dto = postComments.Items.Single(c => c.Id.IdDecode() == comment10.Id);
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
                var animeCommunity = await CommunityTests.CreateCommunityWithRepositoryAsync(
                    _communityRepository, narutoUser, animeCommunityName);
                var postContent = "Sample Post Content";
                var post = await PostTests.CreatePostWithRepositoryAsync(_postRepository, narutoUser,
                    animeCommunity.Id, animeCommunity.Name, postContent);
                var comment1 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1");
                var comment2 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 2");
                var comment3 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3");

                var comment11 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 1", comment1.Id);
                var comment12 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 2", comment1.Id);
                var comment13 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 3", comment1.Id);
                var comment14 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 4", comment1.Id);

                var comment21 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 2, 1", comment2.Id);
                var comment22 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 2, 2", comment2.Id);

                var comment31 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3, 1", comment3.Id);
                var comment32 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3, 2", comment3.Id);
                var comment33 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3, 3", comment3.Id);

                var comment131 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 3, 1", comment13.Id);
                var comment132 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 3, 2", comment13.Id);
                var comment133 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 1, 3, 3", comment13.Id);

                var comment311 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3, 1, 1", comment31.Id);
                var comment312 = await CreateCommentWithRepositoryAsync(_commentRepository, narutoUser, post.Id, "comment 3, 1, 2", comment31.Id);

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

        public static async Task<Comment> CreateCommentWithRepositoryAsync(ICommentRepository commentRepository,
            TestUser user, ulong postId, string content, ulong? parentCommentId = null)
        {
            var newId = await IdGenerator.GenerateNewIdAsync();
            var createdComment = await commentRepository.CreateCommentAsync(newId, user.Id, user.Username, postId, content, parentCommentId);
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

        public static async Task<CommentVote> VoteCommentWithRepositoryAsync(ICommentVoteRepository commentVoteRepository,
            TestUser user, ulong commentId, bool isUp = true)
        {
            var newId = await IdGenerator.GenerateNewIdAsync();
            var commentVote = await commentVoteRepository.CreateCommentVoteAsync(newId, user.Id, user.Username, commentId, isUp);
            Assert.NotNull(commentVote);
            return commentVote;
        }
    }
}

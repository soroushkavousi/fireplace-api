﻿using FireplaceApi.Application.Comments;
using FireplaceApi.Application.Communities;
using FireplaceApi.Application.Posts;
using FireplaceApi.Domain.Comments;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommentTests;

[Collection("Comment")]
public class CommentCreateTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<CommentCreateTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommentOperator _commentOperator;
    private readonly PostOperator _postOperator;
    private readonly CommunityOperator _communityOperator;

    public CommentCreateTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentCreateTests>>();
        _clientPool = _fixture.ClientPool;
        _commentOperator = _fixture.ServiceProvider.GetRequiredService<CommentOperator>();
        _postOperator = _fixture.ServiceProvider.GetRequiredService<PostOperator>();
        _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
    }

    [Fact]
    public async Task CreateComment_WhenCreatingNestedComments_ShouldReturnCorrectNestedComments()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();
            var animeCommunityName = "anime-community";
            var animeCommunity = await _communityOperator.CreateCommunityAsync(narutoUser.Id, animeCommunityName,
                username: narutoUser.Username);
            var postContent = "Sample Post Content";
            var post = await _postOperator.CreatePostAsync(narutoUser.Id,
                animeCommunity.Id, animeCommunity.Name, postContent);

            //When
            var comment1 = await _commentOperator.ReplyToPostAsync(narutoUser.Id, post.Id, "comment 1", username: narutoUser.Username);
            var comment2 = await _commentOperator.ReplyToPostAsync(narutoUser.Id, post.Id, "comment 2", username: narutoUser.Username);
            var comment3 = await _commentOperator.ReplyToPostAsync(narutoUser.Id, post.Id, "comment 3", username: narutoUser.Username);

            var comment11 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment1.Id, "comment 1, 1", postId: post.Id, username: narutoUser.Username);
            var comment12 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment1.Id, "comment 1, 2", postId: post.Id, username: narutoUser.Username);
            var comment13 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment1.Id, "comment 1, 3", postId: post.Id, username: narutoUser.Username);
            var comment14 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment1.Id, "comment 1, 4", postId: post.Id, username: narutoUser.Username);

            var comment21 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment2.Id, "comment 2, 1", postId: post.Id, username: narutoUser.Username);
            var comment22 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment2.Id, "comment 2, 2", postId: post.Id, username: narutoUser.Username);

            var comment31 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment3.Id, "comment 3, 1", postId: post.Id, username: narutoUser.Username);
            var comment32 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment3.Id, "comment 3, 2", postId: post.Id, username: narutoUser.Username);
            var comment33 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment3.Id, "comment 3, 3", postId: post.Id, username: narutoUser.Username);

            var comment131 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment13.Id, "comment 1, 3, 1", postId: post.Id, username: narutoUser.Username);
            var comment132 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment13.Id, "comment 1, 3, 2", postId: post.Id, username: narutoUser.Username);
            var comment133 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment13.Id, "comment 1, 3, 3", postId: post.Id, username: narutoUser.Username);

            var comment311 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment31.Id, "comment 3, 1, 1", postId: post.Id, username: narutoUser.Username);
            var comment312 = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment31.Id, "comment 3, 1, 2", postId: post.Id, username: narutoUser.Username);

            //Then
            var nestedComments = await CommentUtils.ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), CommentSortType.TOP);
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
}

using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.CommentTests;

[Collection("Comment")]
public class CommentListTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<CommentListTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommentOperator _commentOperator;
    private readonly PostOperator _postOperator;
    private readonly CommunityOperator _communityOperator;

    public CommentListTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentListTests>>();
        _clientPool = _fixture.ClientPool;
        _commentOperator = _fixture.ServiceProvider.GetRequiredService<CommentOperator>();
        _postOperator = _fixture.ServiceProvider.GetRequiredService<PostOperator>();
        _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
    }

    [Fact]
    public async Task ListComments_WhenCreatingManyComments_ShouldReturnCorrectQueryResult()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogAppInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();
            var animeCommunityName = "anime-community";
            var animeCommunity = await _communityOperator.CreateCommunityAsync(narutoUser, animeCommunityName);
            var postContent = "Sample Post Content";
            var post = await _postOperator.CreatePostAsync(narutoUser, animeCommunity.Id,
                animeCommunityName, postContent);

            //When
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

            //Then
            var postComments = await CommentUtils.ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), SortType.TOP);
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
}

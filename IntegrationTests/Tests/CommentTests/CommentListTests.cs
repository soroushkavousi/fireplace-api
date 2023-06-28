using FireplaceApi.Application.Comments;
using FireplaceApi.Application.Communities;
using FireplaceApi.Application.Posts;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Communities;
using FireplaceApi.IntegrationTests.Tools;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommentTests;

[Collection("Comment")]
public class CommentListTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<CommentListTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommentOperator _commentOperator;
    private readonly PostOperator _postOperator;
    private readonly ISender _sender;

    public CommentListTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<CommentListTests>>();
        _clientPool = _fixture.ClientPool;
        _commentOperator = _fixture.ServiceProvider.GetRequiredService<CommentOperator>();
        _postOperator = _fixture.ServiceProvider.GetRequiredService<PostOperator>();
        _sender = _fixture.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task ListComments_WhenCreatingManyComments_ShouldReturnCorrectQueryResult()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _logger.LogServerInformation(title: "TEST_START");

            //Given
            var narutoUser = await _clientPool.CreateNarutoUserAsync();
            var animeCommunityName = new CommunityName("anime-community");
            var createCommunityCommand = new CreateCommunityCommand(narutoUser.Id, animeCommunityName, narutoUser.Username);
            var animeCommunity = await _sender.Send(createCommunityCommand);
            var postContent = "Sample Post Content";
            var post = await _postOperator.CreatePostAsync(narutoUser.Id, animeCommunity.Id,
                animeCommunityName, postContent);

            //When
            var comments = new List<Comment>();
            for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 4; i++)
            {
                var comment = await _commentOperator.ReplyToPostAsync(narutoUser.Id, post.Id, $"comment {i}");
                comments.Add(comment);
            }

            var comment10 = comments[9];
            var comment10Vote = await _commentOperator.VoteCommentAsync(narutoUser.Id, comment10.Id, isUp: true);

            var comment10Childs = new List<Comment>();
            for (int i = 0; i < Configs.Current.QueryResult.ViewLimit + 6; i++)
            {
                var comment = await _commentOperator.ReplyToCommentAsync(narutoUser.Id, comment10.Id, $"comment 10, {i}",
                    username: narutoUser.Username, postId: post.Id);
                comment10Childs.Add(comment);
            }

            //Then
            var postComments = await CommentUtils.ListPostCommentsWithApiAsync(narutoUser, post.Id.IdEncode(), CommentSortType.TOP);
            Assert.Equal(Configs.Current.QueryResult.ViewLimit, postComments.Items.Count);
            Assert.Equal(comments.Count - Configs.Current.QueryResult.ViewLimit, postComments.MoreItemIds.Count);

            var comment10Dto = postComments.Items[0];
            Assert.Equal(comment10.Id, comment10Dto.Id.IdDecode());
            Assert.Equal(Configs.Current.QueryResult.ViewLimit, comment10Dto.ChildComments.Count);
            Assert.Equal(comment10Childs.Count - Configs.Current.QueryResult.ViewLimit, comment10Dto.MoreChildCommentIds.Count);

            _logger.LogServerInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogServerCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

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

namespace FireplaceApi.IntegrationTests.Tests.PostTests;

[Collection("Post")]
public class PostCreateTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<PostCreateTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly ISender _sender;

    public PostCreateTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<PostCreateTests>>();
        _clientPool = _fixture.ClientPool;
        _sender = _fixture.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task CreatePost_WhenCreatingAPost_ShouldReturnCreatedPost()
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

            //When
            var createdPost = await PostUtils.CreatePostWithApiAsync(narutoUser, animeCommunityName.Value, postContent);

            //Then
            Assert.Equal(animeCommunityName.Value, createdPost.CommunityName);
            Assert.Equal(postContent, createdPost.Content);

            var retrievedPost = await PostUtils.GetPostWithApiAsync(narutoUser, createdPost.Id);
            Assert.Equal(createdPost.Id, retrievedPost.Id);
            Assert.Equal(animeCommunityName.Value, retrievedPost.CommunityName);
            Assert.Equal(postContent, retrievedPost.Content);

            var animePosts = await PostUtils.ListPostsWithApiAsync(narutoUser, animeCommunityName.Value);
            Assert.Single(animePosts.Items);
            Assert.Equal(createdPost.Id, animePosts.Items[0].Id);

            _logger.LogServerInformation(title: "TEST_END", sw: sw);
        }
        catch (Exception ex)
        {
            _logger.LogServerCritical(title: "TEST_FAILED", sw: sw, ex: ex);
            throw;
        }
    }
}

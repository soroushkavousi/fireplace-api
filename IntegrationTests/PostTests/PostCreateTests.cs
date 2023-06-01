using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Operators;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.PostTests;

[Collection("Post")]
public class PostCreateTests
{
    private readonly ApiIntegrationTestFixture _fixture;
    private readonly ILogger<PostCreateTests> _logger;
    private readonly ClientPool _clientPool;
    private readonly CommunityOperator _communityOperator;

    public PostCreateTests(ApiIntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.CleanDatabase();
        _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<PostCreateTests>>();
        _clientPool = _fixture.ClientPool;
        _communityOperator = _fixture.ServiceProvider.GetRequiredService<CommunityOperator>();
    }

    [Fact]
    public async Task CreatePost_WhenCreatingAPost_ShouldReturnCreatedPost()
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

            //When
            var createdPost = await PostUtils.CreatePostWithApiAsync(narutoUser, animeCommunityName, postContent);

            //Then
            Assert.Equal(animeCommunityName, createdPost.CommunityName);
            Assert.Equal(postContent, createdPost.Content);

            var retrievedPost = await PostUtils.GetPostWithApiAsync(narutoUser, createdPost.Id);
            Assert.Equal(createdPost.Id, retrievedPost.Id);
            Assert.Equal(animeCommunityName, retrievedPost.CommunityName);
            Assert.Equal(postContent, retrievedPost.Content);

            var animePosts = await PostUtils.ListPostsWithApiAsync(narutoUser, animeCommunityName);
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
}

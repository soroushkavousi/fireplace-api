using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Extensions;
using FireplaceApi.IntegrationTests.Extensions;
using FireplaceApi.IntegrationTests.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.PostTests;

public static class PostUtils
{
    public static async Task<PostDto> CreatePostWithApiAsync(TestUser user,
        string communityEncodedIdOrName, string postContent)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"/communities/{communityEncodedIdOrName}/posts")
        {
            Content = new
            {
                content = postContent
            }.ToHttpContent()
        };
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var createdPost = responseBody.FromJson<PostDto>();
        Assert.NotNull(createdPost);
        return createdPost;
    }

    public static async Task<PostDto> GetPostWithApiAsync(TestUser user, string postEncodedId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/posts/{postEncodedId}");
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var retrievedPost = responseBody.FromJson<PostDto>();
        Assert.NotNull(retrievedPost);
        return retrievedPost;
    }

    public static async Task<QueryResultDto<PostDto>> ListPostsWithApiAsync(TestUser user, string communityEncodedIdOrName)
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

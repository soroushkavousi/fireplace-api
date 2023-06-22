using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.IntegrationTests.Extensions;
using FireplaceApi.IntegrationTests.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommentTests;

public static class CommentUtils
{
    private static async Task<CommentDto> ReplyToPostWithApiAsync(TestUser user, string postEncodedId, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/posts/{postEncodedId}/comments")
        {
            Content = content.ToHttpContent()
        };
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var createdComment = responseBody.FromJson<CommentDto>();
        Assert.NotNull(createdComment);
        return createdComment;
    }

    public static async Task<CommentDto> ReplyToCommentWithApiAsync(TestUser user,
        string commentEncodedId, string content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/comments/{commentEncodedId}/comments")
        {
            Content = new
            {
                content
            }.ToHttpContent()
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

    public static async Task<CommentDto> VoteCommentWithApiAsync(TestUser user, string commentEncodedId)
    {
        var url = $"/comments/{commentEncodedId}/votes";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new
            {
                isUpvote = true,
            }.ToHttpContent()
        };
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var comment = responseBody.FromJson<CommentDto>();
        return comment;
    }
}

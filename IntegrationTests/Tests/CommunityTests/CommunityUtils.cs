using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.IntegrationTests.Extensions;
using FireplaceApi.IntegrationTests.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Tests.CommunityTests;

public static class CommunityUtils
{
    public static async Task<CommunityDto> CreateCommunityWithApiAsync(TestUser user, string communityName)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/communities")
        {
            Content = new
            {
                name = communityName
            }.ToHttpContent()
        };
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var createdCommunity = responseBody.FromJson<CommunityDto>();
        Assert.NotNull(createdCommunity);
        return createdCommunity;
    }

    public static async Task<CommunityDto> GetCommunityWithApiAsync(TestUser user, string communityName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/communities/{communityName}");
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var retrievedCommunity = responseBody.FromJson<CommunityDto>();
        Assert.NotNull(retrievedCommunity);
        return retrievedCommunity;
    }

    public static async Task<QueryResultDto<CommunityDto>> ListCommunitiesWithApiAsync(TestUser user, string communityName)
    {
        var baseUrl = $"/communities";
        var queryParameters = new Dictionary<string, string>()
            {
                { "search", communityName },
            };
        var requestUrl = QueryHelpers.AddQueryString(baseUrl, queryParameters);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var response = await user.SendRequestAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var queryResult = responseBody.FromJson<QueryResultDto<CommunityDto>>();
        return queryResult;
    }
}

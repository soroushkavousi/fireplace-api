using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using NLog;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.IntegrationTests.Extensions;

public static class TestExtensions
{
    private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

    public static async Task EnsureResponseIsTheErrorAsync(this HttpResponseMessage response,
        ErrorType expectedErrorType, FieldName expectedRrrorField,
        [CallerMemberName] string testName = "")
        => await response.EnsureResponseIsTheErrorAsync(expectedErrorType.Name, expectedRrrorField.Name, testName);

    public static async Task EnsureResponseIsTheErrorAsync(this HttpResponseMessage response,
        string expectedErrorType, string expectedRrrorField,
        [CallerMemberName] string testName = "")
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        _logger.LogAppInformation($"responseBody: {responseBody}", title: testName);

        var actualError = responseBody.FromJson<ApiExceptionErrorDto>();
        Assert.NotNull(actualError.Code);
        Assert.NotNull(actualError.Message);
        Assert.Equal(expectedErrorType, actualError.Type);
        Assert.Equal(expectedRrrorField, actualError.Field);
    }

    public static async Task EnsureResponseIsNotTheErrorAsync(this HttpResponseMessage response,
        ErrorType notExpectedErrorType, FieldName notExpectedRrrorField,
        [CallerMemberName] string testName = "")
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        _logger.LogAppInformation($"responseBody: {responseBody}", title: testName);

        var actualError = responseBody.FromJson<ErrorDto>();
        Assert.NotEqual(notExpectedErrorType.Name, actualError.Type);
        Assert.NotEqual(notExpectedRrrorField.Name, actualError.Field);
    }

    public static HttpContent ToHttpContent(this object requestContent, string contentType = "application/json")
        => requestContent.ToJson(ignoreSensitiveLimit: true).ToHttpContent(contentType);

    public static HttpContent ToHttpContent(this string requestContent, string contentType = "application/json")
        => new StringContent(requestContent, Encoding.UTF8, contentType);

    public static async Task<T> ReadBodyAsync<T>(this HttpResponseMessage response)
    {
        var responseBodyString = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(responseBodyString));
        return responseBodyString.FromJson<T>();
    }
}

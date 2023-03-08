using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Application.IntegrationTests.Tools
{
    public class TestUtils
    {
        private readonly ILogger<TestUtils> _logger;
        private readonly ErrorOperator _errorOperator;

        public object SampleObject { get; }

        public TestUtils(ApiIntegrationTestFixture testFixture)
        {
            _logger = testFixture.ServiceProvider.GetRequiredService<ILogger<TestUtils>>();
            _errorOperator = testFixture.ServiceProvider.GetRequiredService<ErrorOperator>();
            SampleObject = new { Property1 = "value1" };
        }

        public async Task AssertResponseIsErrorAsync(ErrorType expectedErrorType, FieldName expectedRrrorField,
            HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBodyJObject = await AssertResponseBodyIsJObject(response);
            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);
            _logger.LogAppInformation($"responseBodyJson: {responseBodyJson}");

            Assert.Contains("code", responseBodyJObject);
            Assert.Contains("type", responseBodyJObject);
            Assert.Contains("field", responseBodyJObject);
            Assert.Contains("message", responseBodyJObject);

            var code = responseBodyJObject.Value<int>("code");
            var responseError = await _errorOperator.GetErrorAsync(ErrorIdentifier.OfCode(code));
            Assert.True(responseError.Type == expectedErrorType
                && responseError.Field == expectedRrrorField);
        }

        public async Task AssertResponseIsNotErrorAsync(ErrorType notExpectedErrorType, FieldName notExpectedRrrorField,
            HttpResponseMessage response, [CallerMemberName] string testName = "")
        {
            var responseBodyJToken = await ReadResponseBodyAsJToken(response);
            if (responseBodyJToken.Type != JTokenType.Object)
                return;

            var responseBodyJObject = responseBodyJToken.To<JObject>();

            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);
            _logger.LogAppInformation($"responseBodyJson: {responseBodyJson}");

            var responseErrorCode = responseBodyJObject.Value<int?>("code");
            if (responseErrorCode.HasValue)
            {
                var responseError = await _errorOperator.GetErrorAsync(ErrorIdentifier.OfCode(responseErrorCode.Value));
                Assert.False(responseError.Type == notExpectedErrorType
                && responseError.Field == notExpectedRrrorField);
            }
        }

        public HttpContent MakeRequestContent(object requestContent, string contentType = "application/json")
            => MakeRequestContent(requestContent.ToJson(ignoreSensitiveLimit: true), contentType);

        public HttpContent MakeRequestContent(string requestContent, string contentType = "application/json")
            => new StringContent(requestContent, Encoding.UTF8, contentType);

        public async Task<JToken> ReadResponseBodyAsJToken(HttpResponseMessage response)
        {
            var responseBodyString = await response.Content.ReadAsStringAsync();
            Assert.False(responseBodyString.IsNullOrEmpty());
            var responseBodyJToken = JToken.Parse(responseBodyString);
            return responseBodyJToken;
        }

        public async Task<JObject> AssertResponseBodyIsJObject(HttpResponseMessage response)
        {
            var responseBodyJToken = await ReadResponseBodyAsJToken(response);
            Assert.Equal(JTokenType.Object, responseBodyJToken.Type);
            var responseBodyJObject = responseBodyJToken.To<JObject>();
            return responseBodyJObject;
        }
    }
}

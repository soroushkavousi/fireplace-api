using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Operators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GamingCommunityApi.Api.IntegrationTests.Tools
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

        public async Task AssertResponseContainsErrorAsync(ErrorName expectedErrorName, HttpResponseMessage response, string testName)
        {
            _logger.LogInformation($"{testName} | Checking response status code is bad request. ({response.StatusCode})");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBodyJObject = await ReadResponseBodyAsJObject(response, testName);
            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);

            _logger.LogInformation($"{testName} | Checking response body contains code. ({responseBodyJson})");
            Assert.Contains("code", responseBodyJObject);

            _logger.LogInformation($"{testName} | Checking response body contains message. ({responseBodyJson})");
            Assert.Contains("message", responseBodyJObject);

            var responseError = await _errorOperator.GetErrorByCodeAsync(responseBodyJObject.Value<int>("code"));

            _logger.LogInformation($"{testName} | Checking response is {expectedErrorName.ToString()}. ({responseError.Name.ToString()})");
            Assert.Equal(expectedErrorName, responseError.Name);
        }

        public async Task AssertResponseDoesNotContainErrorAsync(ErrorName notExpectedErrorName, HttpResponseMessage response, string testName)
        {
            var responseBodyJObject = await ReadResponseBodyAsJObject(response, testName);
            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);
            _logger.LogInformation($"{testName} | responseBodyJson: {responseBodyJson}");

            var responseErrorCode = responseBodyJObject.Value<int?>("code");
            if (responseErrorCode.HasValue)
            {
                var responseError = await _errorOperator.GetErrorByCodeAsync(responseErrorCode.Value);

                _logger.LogInformation($"{testName} | Checking response is not {notExpectedErrorName.ToString()}. ({responseError.Name.ToString()})");
                Assert.NotEqual(notExpectedErrorName, responseError.Name);
            }
        }

        public HttpContent MakeRequestContent(HttpMethod httpMethod, object requestContent)
        {
            if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
                return new StringContent(requestContent.ToJson(), Encoding.UTF8, "application/json");
            else if (httpMethod == HttpMethod.Patch)
                return new StringContent(requestContent.ToJson(), Encoding.UTF8, "application/merge-patch+json");
            else
                return null;
        }

        public async Task<JObject> ReadResponseBodyAsJObject(HttpResponseMessage response, string testName)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"{testName} | Checking response body is not null. ({responseBody})");
            Assert.NotNull(responseBody);

            var responseBodyJObject = JObject.Parse(responseBody);
            return responseBodyJObject;
        }
    }
}

using FireplaceApi.Api.IntegrationTests.Tools;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FireplaceApi.Api.IntegrationTests
{
    [Collection("Api Integration Test Collection")]
    public class FirewallTests
    {
        private readonly ILogger<FirewallTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public FirewallTests(ApiIntegrationTestFixture testFixture)
        {
            _logger = testFixture.ServiceProvider.GetRequiredService<ILogger<FirewallTests>>();
            _clientPool = testFixture.ClientPool;
            _testUtils = testFixture.TestUtils;
        }

        [InlineData("GET", "/v0.1/access-tokens/some-token")]

        [InlineData("POST", "/v0.1/emails/999/activate")]
        [InlineData("GET", "/v0.1/emails/999")]

        [InlineData("GET", "/v0.1/errors")]
        [InlineData("GET", "/v0.1/errors/999")]
        [InlineData("PATCH", "/v0.1/errors/999")]

        [InlineData("POST", "/v0.1/files")]

        [InlineData("POST", "/v0.1/sessions/999/revoke")]
        [InlineData("GET", "/v0.1/sessions")]
        [InlineData("GET", "/v0.1/sessions/999")]

        [InlineData("GET", "/v0.1/users")]
        [InlineData("GET", "/v0.1/users/999")]
        [InlineData("PATCH", "/v0.1/users/999")]
        [InlineData("DELETE", "/v0.1/users/999")]
        [Theory]
        public async Task TestFirewallCheckContentType(
            string httpMethodName, string requestUri)
        {
            _logger.LogInformation($"{nameof(TestFirewallCheckContentType)} | Start | ({httpMethodName}, {requestUri})");

            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri);
            var response = await _clientPool.GuestClient.SendAsync(request);

            if (httpMethod == HttpMethod.Post
                || httpMethod == HttpMethod.Put
                || httpMethod == HttpMethod.Patch)
            {
                await _testUtils.AssertResponseContainsErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));

                request = new HttpRequestMessage(httpMethod, requestUri)
                {
                    Content = _testUtils.MakeRequestContent(httpMethod, _testUtils.SampleObject)
                };
                response = await _clientPool.GuestClient.SendAsync(request);
                await _testUtils.AssertResponseDoesNotContainErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));
            }
            else
            {
                await _testUtils.AssertResponseDoesNotContainErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));
            }

            _logger.LogInformation($"{nameof(TestFirewallCheckContentType)} | End");
        }

        [InlineData("GET", "/v0.1/access-tokens/some-token")]

        [InlineData("POST", "/v0.1/emails/999/activate")]
        [InlineData("GET", "/v0.1/emails/999")]

        [InlineData("GET", "/v0.1/errors")]
        [InlineData("GET", "/v0.1/errors/999")]
        [InlineData("PATCH", "/v0.1/errors/999")]

        [InlineData("POST", "/v0.1/files")]

        [InlineData("POST", "/v0.1/sessions/999/revoke")]
        [InlineData("GET", "/v0.1/sessions")]
        [InlineData("GET", "/v0.1/sessions/999")]

        [InlineData("GET", "/v0.1/users")]
        [InlineData("GET", "/v0.1/users/999")]
        [InlineData("PATCH", "/v0.1/users/999")]
        [InlineData("DELETE", "/v0.1/users/999")]
        [Theory]
        public async Task TestGuestCantAccessToPrivateMethods(
            string httpMethodName, string requestUri)
        {
            _logger.LogInformation($"{nameof(TestGuestCantAccessToPrivateMethods)} | Start | ({httpMethodName}, {requestUri})");
            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = _testUtils.MakeRequestContent(httpMethod, _testUtils.SampleObject)
            };

            var response = await _clientPool.GuestClient.SendAsync(request);
            await _testUtils.AssertResponseContainsErrorAsync(ErrorName.AUTHENTICATION_FAILED, response, nameof(TestGuestCantAccessToPrivateMethods));
            _logger.LogInformation($"{nameof(TestGuestCantAccessToPrivateMethods)} | End");
        }

        [InlineData("POST", "/v0.1/users/sign-up-with-email")]
        [InlineData("POST", "/v0.1/users/log-in-with-email")]
        [InlineData("POST", "/v0.1/users/log-in-with-username")]
        [Theory]
        public async Task TestGuestCanAccessToPublicMethods(
            string httpMethodName, string requestUri)
        {
            _logger.LogInformation($"{nameof(TestGuestCanAccessToPublicMethods)} | Start | ({httpMethodName}, {requestUri})");
            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = _testUtils.MakeRequestContent(httpMethod, _testUtils.SampleObject)
            };

            var response = await _clientPool.GuestClient.SendAsync(request);
            await _testUtils.AssertResponseDoesNotContainErrorAsync(ErrorName.AUTHENTICATION_FAILED, response, nameof(TestGuestCantAccessToPrivateMethods));
            _logger.LogInformation($"{nameof(TestGuestCanAccessToPublicMethods)} | End");
        }

        [InlineData("GET", "/v0.1/access-tokens/some-token")]

        [InlineData("POST", "/v0.1/emails/999/activate")]
        [InlineData("GET", "/v0.1/emails/999")]

        [InlineData("GET", "/v0.1/errors")]
        [InlineData("GET", "/v0.1/errors/999")]
        [InlineData("PATCH", "/v0.1/errors/999")]

        [InlineData("POST", "/v0.1/files")]

        [InlineData("POST", "/v0.1/sessions/999/revoke")]
        [InlineData("GET", "/v0.1/sessions")]
        [InlineData("GET", "/v0.1/sessions/999")]

        [InlineData("GET", "/v0.1/users")]
        [InlineData("GET", "/v0.1/users/999")]
        [InlineData("PATCH", "/v0.1/users/999")]
        [InlineData("DELETE", "/v0.1/users/999")]
        [Theory]
        public async Task TestTheHulkCanAccessWithAuthentication(
            string httpMethodName, string requestUri)
        {
            _logger.LogInformation($"{nameof(TestTheHulkCanAccessWithAuthentication)} | Start | ({httpMethodName}, {requestUri})");
            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = _testUtils.MakeRequestContent(httpMethod, _testUtils.SampleObject)
            };

            var response = await _clientPool.TheHulkClient.SendAsync(request);
            await _testUtils.AssertResponseDoesNotContainErrorAsync(ErrorName.AUTHENTICATION_FAILED, response, nameof(TestTheHulkCanAccessWithAuthentication));
            _logger.LogInformation($"{nameof(TestTheHulkCanAccessWithAuthentication)} | End");
        }
    }
}

using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GamingCommunityApi.Api.IntegrationTests
{
    public class FirewallTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _testFixture;

        public FirewallTests(ApiIntegrationTestFixture apiIntegrationTestFixture)
        {
            _testFixture = apiIntegrationTestFixture;
            _testFixture.InitialGuestClient();
        }

        [InlineData("GET", "/v0.1/access-tokens/some-token")]

        [InlineData("POST", "/v0.1/emails/1/activate")]
        [InlineData("GET", "/v0.1/emails/1")]

        [InlineData("GET", "/v0.1/errors")]
        [InlineData("GET", "/v0.1/errors/1")]
        [InlineData("PATCH", "/v0.1/errors/1")]

        [InlineData("POST", "/v0.1/files")]

        [InlineData("POST", "/v0.1/sessions/1/revoke")]
        [InlineData("GET", "/v0.1/sessions")]
        [InlineData("GET", "/v0.1/sessions/1")]

        [InlineData("GET", "/v0.1/users")]
        [InlineData("GET", "/v0.1/users/1")]
        [InlineData("PATCH", "/v0.1/users/1")]
        [InlineData("DELETE", "/v0.1/users/1")]
        [Theory]
        public async Task TestFirewallCheckContentType(
            string httpMethodName, string requestUri)
        {
            _testFixture.Logger.LogInformation($"{nameof(TestFirewallCheckContentType)} | Start | ({httpMethodName}, {requestUri})");

            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri);
            var response = await _testFixture.GuestClient.SendAsync(request);

            if (httpMethod == HttpMethod.Post
                || httpMethod == HttpMethod.Put
                || httpMethod == HttpMethod.Patch)
            {
                await _testFixture.AssertResponseContainsErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));

                request = new HttpRequestMessage(httpMethod, requestUri)
                {
                    Content = _testFixture.MakeRequestContent(httpMethod, _testFixture.SampleObject)
                };
                response = await _testFixture.GuestClient.SendAsync(request);
                await _testFixture.AssertResponseDoesNotContainErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));
            }
            else
            {
                await _testFixture.AssertResponseDoesNotContainErrorAsync(ErrorName.REQUEST_CONTENT_TYPE_IS_NOT_VALID, response, nameof(TestFirewallCheckContentType));
            }

            _testFixture.Logger.LogInformation($"{nameof(TestFirewallCheckContentType)} | End");
        }

        [InlineData("GET", "/v0.1/access-tokens/some-token")]

        [InlineData("POST", "/v0.1/emails/1/activate")]
        [InlineData("GET", "/v0.1/emails/1")]

        [InlineData("GET", "/v0.1/errors")]
        [InlineData("GET", "/v0.1/errors/1")]
        [InlineData("PATCH", "/v0.1/errors/1")]

        [InlineData("POST", "/v0.1/files")]

        [InlineData("POST", "/v0.1/sessions/1/revoke")]
        [InlineData("GET", "/v0.1/sessions")]
        [InlineData("GET", "/v0.1/sessions/1")]

        [InlineData("GET", "/v0.1/users")]
        [InlineData("GET", "/v0.1/users/1")]
        [InlineData("PATCH", "/v0.1/users/1")]
        [InlineData("DELETE", "/v0.1/users/1")]
        [Theory]
        public async Task TestGuestClientCantAccessToPrivateMethods(
            string httpMethodName, string requestUri)
        {
            _testFixture.Logger.LogInformation($"{nameof(TestGuestClientCantAccessToPrivateMethods)} | Start | ({httpMethodName}, {requestUri})");
            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = _testFixture.MakeRequestContent(httpMethod, _testFixture.SampleObject)
            };

            var response = await _testFixture.GuestClient.SendAsync(request);
            await _testFixture.AssertResponseContainsErrorAsync(ErrorName.AUTHENTICATION_FAILED, response, nameof(TestGuestClientCantAccessToPrivateMethods));
            _testFixture.Logger.LogInformation($"{nameof(TestGuestClientCantAccessToPrivateMethods)} | End");
        }

        [InlineData("POST", "/v0.1/users/sign-up-with-email")]
        [InlineData("POST", "/v0.1/users/log-in-with-email")]
        [InlineData("POST", "/v0.1/users/log-in-with-username")]
        [Theory]
        public async Task TestGuestClientCanAccessToPublicMethods(
            string httpMethodName, string requestUri)
        {
            _testFixture.Logger.LogInformation($"{nameof(TestGuestClientCanAccessToPublicMethods)} | Start | ({httpMethodName}, {requestUri})");
            var httpMethod = new HttpMethod(httpMethodName);
            var request = new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = _testFixture.MakeRequestContent(httpMethod, _testFixture.SampleObject)
            };

            var response = await _testFixture.GuestClient.SendAsync(request);
            await _testFixture.AssertResponseDoesNotContainErrorAsync(ErrorName.AUTHENTICATION_FAILED, response, nameof(TestGuestClientCantAccessToPrivateMethods));
            _testFixture.Logger.LogInformation($"{nameof(TestGuestClientCanAccessToPublicMethods)} | End");
        }

        public async Task TestUserClientsCanAccessWithAuthentication(
            string httpMethodName, string requestUri)
        {

        }
    }
}

using FireplaceApi.Application.Enums;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Application.IntegrationTests
{
    public class FirewallTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _fixture;
        private readonly ILogger<FirewallTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public FirewallTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<FirewallTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
        }


        [Fact]
        public async Task Guest_TryToAccessPrivateEndpoint_AccessDenied()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var request = new HttpRequestMessage(HttpMethod.Get, "/users/me");
                var httpClient = _clientPool.CreateGuestHttpClientAsync();
                var response = await httpClient.SendAsync(request);
                await _testUtils.AssertResponseIsErrorAsync(ErrorType.AUTHENTICATION_FAILED, FieldName.ACCESS_TOKEN, response);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        [Fact]
        public async Task User_TryToAccessPrivateEndpoint_AccessAccept()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var request = new HttpRequestMessage(HttpMethod.Get, "/users/me");
                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var response = await narutoUser.SendRequestAsync(request);
                await _testUtils.AssertResponseIsNotErrorAsync(ErrorType.AUTHENTICATION_FAILED, FieldName.ACCESS_TOKEN, response);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        [Fact]
        public async Task User_SendEmptyBody_InvalidContentType()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/communities");
                var response = await narutoUser.SendRequestAsync(request);
                await _testUtils.AssertResponseIsErrorAsync(ApplicationErrorType.INVALID_VALUE,
                    ApplicationFieldName.REQUEST_CONTENT_TYPE, response);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }

        [Theory]
        [InlineData("{name:\"test-community-name\"}")]
        public async Task User_SendBodyWithWrongJsonFormat_BodyIsNotJsonError(
            string jsonWithWrongFormat)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START", parameters: new { jsonWithWrongFormat });

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "/communities")
                {
                    Content = _testUtils.MakeRequestContent(jsonWithWrongFormat),
                };
                var response = await narutoUser.SendRequestAsync(request);
                await _testUtils.AssertResponseIsErrorAsync(ApplicationErrorType.INVALID_VALUE,
                    ApplicationFieldName.REQUEST_BODY, response);

                _logger.LogAppInformation(title: "TEST_END", sw: sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical(title: "TEST_FAILED", sw: sw, ex: ex);
                throw;
            }
        }
    }
}

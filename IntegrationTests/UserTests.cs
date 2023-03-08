using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.IntegrationTests.Tools;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FireplaceApi.Application.IntegrationTests
{
    public class UserTests : IClassFixture<ApiIntegrationTestFixture>
    {
        private readonly ApiIntegrationTestFixture _fixture;
        private readonly ILogger<UserTests> _logger;
        private readonly ClientPool _clientPool;
        private readonly TestUtils _testUtils;

        public UserTests(ApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.CleanDatabase();
            _logger = _fixture.ServiceProvider.GetRequiredService<ILogger<UserTests>>();
            _clientPool = _fixture.ClientPool;
            _testUtils = _fixture.TestUtils;
        }

        [Fact]
        public async Task User_LogInWithEmail_SuccessfulLogin()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _logger.LogAppInformation(title: "TEST_START");

                var narutoUser = await _clientPool.CreateNarutoUserAsync();
                var emailAddress = narutoUser.Email.Address;
                var password = narutoUser.Password.Value;
                var request = new HttpRequestMessage(HttpMethod.Post, "/users/log-in-with-email")
                {
                    Content = _testUtils.MakeRequestContent(new
                    {
                        emailAddress,
                        password
                    })
                };
                var response = await narutoUser.SendRequestAsync(request);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var loggedInUser = responseBody.FromJson<UserDto>();
                Assert.NotNull(loggedInUser);
                Assert.False(string.IsNullOrWhiteSpace(loggedInUser.AccessToken));
                var loggedInUserId = loggedInUser.Id.IdDecode();
                Assert.Equal(narutoUser.Id, loggedInUserId);

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

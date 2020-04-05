using GamingCommunityApi.Api.Tools;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GamingCommunityApi.Api.IntegrationTests
{
    public class ApiIntegrationTestFixture : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _apiFactory;
        private readonly WebApplicationFactoryClientOptions _clientOptions;
        
        public HttpClient GuestClient { get; set; }
        public HttpClient NotVerifiedUserClient { get; set; }
        public ILogger<ApiIntegrationTestFixture> Logger { get; }
        public IServiceProvider ServiceProvider { get; }
        public GamingCommunityApiContext GamingCommunityApiContext { get; }
        public ErrorOperator ErrorOperator { get; }
        public object SampleObject { get; }

        public ApiIntegrationTestFixture()
        {
            var logger = Utils.SetupLogger();
            _apiFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        //config.AddJsonFile(
                        //    "something.json", optional: false, reloadOnChange: true);
                    });
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<GamingCommunityApiContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        var sp = services.BuildServiceProvider();
                        var configuration = sp.GetRequiredService<IConfiguration>();
                        services.AddDbContext<GamingCommunityApiContext>(
                            optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("TestDatabase"))
                        );
                    });
                    builder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        //logging.AddXUnit(output);
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }).UseNLog();
                });

            _clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost:5021"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            var sp = _apiFactory.Services;
            var scope = sp.CreateScope();
            ServiceProvider = scope.ServiceProvider;
            GamingCommunityApiContext = ServiceProvider.GetRequiredService<GamingCommunityApiContext>();
            Logger = ServiceProvider.GetRequiredService<ILogger<ApiIntegrationTestFixture>>();
            ErrorOperator = ServiceProvider.GetRequiredService<ErrorOperator>();

            SampleObject = new { Property1 = "value1" };

            Logger.LogInformation($"ApiIntegrationTestFixture initialized successfully.");
        }

        public void InitialGuestClient()
        {
            GuestClient = _apiFactory.CreateClient(_clientOptions);
        }
        
        public async Task AssertResponseContainsErrorAsync(ErrorName expectedErrorName, HttpResponseMessage response, string testName)
        {
            Logger.LogInformation($"{testName} | Checking response status code is bad request. ({response.StatusCode})");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBodyJObject = await ReadResponseBodyAsJObject(response, testName);
            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);

            Logger.LogInformation($"{testName} | Checking response body contains code. ({responseBodyJson})");
            Assert.Contains("code", responseBodyJObject);

            Logger.LogInformation($"{testName} | Checking response body contains message. ({responseBodyJson})");
            Assert.Contains("message", responseBodyJObject);

            var responseError = await ErrorOperator.GetErrorByCodeAsync(responseBodyJObject.Value<int>("code"));

            Logger.LogInformation($"{testName} | Checking response is {expectedErrorName.ToString()}. ({responseError.Name.ToString()})");
            Assert.Equal(expectedErrorName, responseError.Name);
        }

        public async Task AssertResponseDoesNotContainErrorAsync(ErrorName notExpectedErrorName, HttpResponseMessage response, string testName)
        {
            var responseBodyJObject = await ReadResponseBodyAsJObject(response, testName);
            var responseBodyJson = responseBodyJObject.ToString(Formatting.None);
            Logger.LogInformation($"{testName} | responseBodyJson: {responseBodyJson}");

            var responseErrorCode = responseBodyJObject.Value<int?>("code");
            if(responseErrorCode.HasValue)
            {
                var responseError = await ErrorOperator.GetErrorByCodeAsync(responseErrorCode.Value);

                Logger.LogInformation($"{testName} | Checking response is not {notExpectedErrorName.ToString()}. ({responseError.Name.ToString()})");
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

            Logger.LogInformation($"{testName} | Checking response body is not null. ({responseBody})");
            Assert.NotNull(responseBody);

            var responseBodyJObject = JObject.Parse(responseBody);
            return responseBodyJObject;
        }

        public void Dispose()
        {
            GamingCommunityApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE ""UserEntities"" CASCADE;");
        }
    }
}

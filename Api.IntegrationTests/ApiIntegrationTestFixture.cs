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
                BaseAddress = new Uri("http://localhost:5020"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            var sp = _apiFactory.Services;
            var scope = sp.CreateScope();
            ServiceProvider = scope.ServiceProvider;
            GamingCommunityApiContext = ServiceProvider.GetRequiredService<GamingCommunityApiContext>();
            Logger = ServiceProvider.GetRequiredService<ILogger<ApiIntegrationTestFixture>>();
            ErrorOperator = ServiceProvider.GetRequiredService<ErrorOperator>();

            Logger.LogInformation($"ApiIntegrationTestFixture initialized successfully.");
        }

        public void InitialGuestClient()
        {
            GuestClient = _apiFactory.CreateClient(_clientOptions);
        }

        public async Task AssertResponseHasErrorNameAsync(ErrorName expectedErrorName, HttpResponseMessage response, string testName)
        {
            Logger.LogInformation($"{testName} | Checking response.StatusCode is bad request. ({response.StatusCode})");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseJsonObject = responseBody.FromJson<dynamic>();
            string responseJsonString = responseJsonObject.ToString();
            responseJsonString = responseJsonString.RemoveLineBreaks();

            Logger.LogInformation($"{testName} | Checking response body contains code. ({responseJsonString})");
            Assert.Contains("code", responseJsonObject);

            Logger.LogInformation($"{testName} | Checking response body contains message. ({responseJsonString})");
            Assert.Contains("message", responseJsonObject);

            int responseErrorCode = responseJsonObject["code"];
            var responseError = await ErrorOperator.GetErrorByCodeAsync(responseErrorCode);

            Logger.LogInformation($"{testName} | Checking response is {expectedErrorName.ToString()}. ({responseError.Name.ToString()})");
            Assert.Equal(expectedErrorName, responseError.Name);
        }

        public HttpContent GetRequestHttpContentFromDynamic(dynamic dynamicObject)
        {
            return new StringContent(dynamicObject.ToJson(), Encoding.UTF8, "application/json");
        }

        public void Dispose()
        {
            GamingCommunityApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE ""UserEntities"" CASCADE;");
        }
    }
}

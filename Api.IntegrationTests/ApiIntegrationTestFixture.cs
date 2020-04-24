using GamingCommunityApi.Api.IntegrationTests.Tools;
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
    [CollectionDefinition("Api Integration Test Collection")]
    public class DatabaseCollection : ICollectionFixture<ApiIntegrationTestFixture> { }

    public class ApiIntegrationTestFixture : IDisposable
    {
        private readonly ILogger<ApiIntegrationTestFixture> _logger;
        private readonly IConfiguration _configuration;
        private readonly GamingCommunityApiContext _gamingCommunityApiContext;
        private readonly ErrorOperator _errorOperator;

        public WebApplicationFactory<Startup> ApiFactory;
        public IServiceProvider ServiceProvider { get; }
        public ClientPool ClientPool { get; }
        public TestUtils TestUtils { get; }

        public ApiIntegrationTestFixture()
        {
            var logger = Utils.SetupLogger();
            ApiFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        //config.AddJsonFile(
                        //    "something.json", optional: false, reloadOnChange: true);
                    });
                    builder.ConfigureServices(services =>
                    {
                        UseTestDatabase(services);
                    });
                    builder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        //logging.AddXUnit(output);
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }).UseNLog();
                });

            var sp = ApiFactory.Services;
            var scope = sp.CreateScope();
            ServiceProvider = scope.ServiceProvider;
            _logger = ServiceProvider.GetRequiredService<ILogger<ApiIntegrationTestFixture>>();
            _configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            _gamingCommunityApiContext = ServiceProvider.GetRequiredService<GamingCommunityApiContext>();
            _errorOperator = ServiceProvider.GetRequiredService<ErrorOperator>();

            ClearTestDatabase();
            InitTestDatabase();
            ClientPool = new ClientPool(this);
            TestUtils = new TestUtils(this);

            _logger.LogInformation($"ApiIntegrationTestFixture initialized successfully.");
        }

        private void InitTestDatabase()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<GamingCommunityApiContext>();
            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("MainDatabase"));
            var mainGamingCommunityApiContext = new GamingCommunityApiContext(_configuration.GetConnectionString("MainDatabase"));

            var errorEntities = mainGamingCommunityApiContext.ErrorEntities.AsNoTracking().ToList();
            var globalEntities = mainGamingCommunityApiContext.GlobalEntities.AsNoTracking().ToList();

            _gamingCommunityApiContext.ErrorEntities.AddRange(errorEntities);
            _gamingCommunityApiContext.GlobalEntities.AddRange(globalEntities);
            _gamingCommunityApiContext.SaveChanges();
            _gamingCommunityApiContext.DetachAllEntries();
        }


        private void UseTestDatabase(IServiceCollection services)
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
        }

        public void ClearTestDatabase()
        {
            _gamingCommunityApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""GlobalEntities"" CASCADE;");
            _gamingCommunityApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""ErrorEntities"" CASCADE;");
            _gamingCommunityApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""UserEntities"" CASCADE;");
        }

        public void Dispose()
        {
            ClearTestDatabase();
        }
    }
}

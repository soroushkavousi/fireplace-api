using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace FireplaceApi.Api.IntegrationTests
{
    [CollectionDefinition("Api Integration Test Collection")]
    public class DatabaseCollection : ICollectionFixture<ApiIntegrationTestFixture> { }

    public class ApiIntegrationTestFixture : IDisposable
    {
        private readonly ILogger<ApiIntegrationTestFixture> _logger;

        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly ErrorOperator _errorOperator;

        public WebApplicationFactory<Program> ApiFactory;
        public IServiceProvider ServiceProvider { get; }
        public ClientPool ClientPool { get; }
        public TestUtils TestUtils { get; }

        public ApiIntegrationTestFixture()
        {
            var sw = Stopwatch.StartNew();
            ProjectInitializer.Start();
            var logger = ProjectInitializer.Logger;
            ApiFactory = new WebApplicationFactory<Program>()
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
            _fireplaceApiContext = ServiceProvider.GetRequiredService<FireplaceApiContext>();
            _errorOperator = ServiceProvider.GetRequiredService<ErrorOperator>();

            ClearTestDatabase();
            InitTestDatabase();
            ClientPool = new ClientPool(this);
            TestUtils = new TestUtils(this);

            _logger.LogAppInformation($"ApiIntegrationTestFixture initialized successfully.", sw);
        }

        private void InitTestDatabase()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<FireplaceApiContext>();
            //optionsBuilder.UseNpgsql(Configs.Instance.Database.MainConnectionString);
            var mainFireplaceApiContext = new FireplaceApiContext(Configs.Instance.Database.MainConnectionString);

            var errorEntities = mainFireplaceApiContext.ErrorEntities.AsNoTracking().ToList();

            _fireplaceApiContext.ErrorEntities.AddRange(errorEntities);
            _fireplaceApiContext.SaveChanges();
            _fireplaceApiContext.DetachAllEntries();
        }


        private void UseTestDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<FireplaceApiContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var sp = services.BuildServiceProvider();
            var configuration = sp.GetRequiredService<IConfiguration>();
            services.AddDbContext<FireplaceApiContext>(
                optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString(Tools.Constants.TestDatabaseKey))
            );
        }

        public void ClearTestDatabase()
        {
            _fireplaceApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""GlobalEntities"" CASCADE;");
            _fireplaceApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""ErrorEntities"" CASCADE;");
            _fireplaceApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""UserEntities"" CASCADE;");
        }

        public void Dispose()
        {
            ClearTestDatabase();
        }
    }
}

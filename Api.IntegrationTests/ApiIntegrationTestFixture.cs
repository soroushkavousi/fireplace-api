using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Operators;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Linq;
using Xunit;

namespace FireplaceApi.Api.IntegrationTests
{
    [CollectionDefinition("Api Integration Test Collection")]
    public class DatabaseCollection : ICollectionFixture<ApiIntegrationTestFixture> { }

    public class ApiIntegrationTestFixture : IDisposable
    {
        private readonly ILogger<ApiIntegrationTestFixture> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly ErrorOperator _errorOperator;

        public WebApplicationFactory<Startup> ApiFactory;
        public IServiceProvider ServiceProvider { get; }
        public ClientPool ClientPool { get; }
        public TestUtils TestUtils { get; }

        public ApiIntegrationTestFixture()
        {
            ProjectInitializer.Start();
            var logger = ProjectInitializer.Logger;
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
            _fireplaceApiContext = ServiceProvider.GetRequiredService<FireplaceApiContext>();
            _errorOperator = ServiceProvider.GetRequiredService<ErrorOperator>();

            ClearTestDatabase();
            InitTestDatabase();
            ClientPool = new ClientPool(this);
            TestUtils = new TestUtils(this);

            _logger.LogInformation($"ApiIntegrationTestFixture initialized successfully.");
        }

        private void InitTestDatabase()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<FireplaceApiContext>();
            //optionsBuilder.UseNpgsql(_configuration.GetConnectionString("MainDatabase"));
            var mainFireplaceApiContext = new FireplaceApiContext(_configuration.GetConnectionString("MainDatabase"));

            var errorEntities = mainFireplaceApiContext.ErrorEntities.AsNoTracking().ToList();
            var globalEntities = mainFireplaceApiContext.GlobalEntities.AsNoTracking().ToList();

            _fireplaceApiContext.ErrorEntities.AddRange(errorEntities);
            _fireplaceApiContext.GlobalEntities.AddRange(globalEntities);
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
                optionsBuilder => optionsBuilder.UseNpgsql(configuration.GetConnectionString("TestDatabase"))
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

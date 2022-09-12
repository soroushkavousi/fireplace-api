using FireplaceApi.Api.IntegrationTests.Tools;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FireplaceApi.Api.IntegrationTests
{
    public class ApiIntegrationTestFixture : IDisposable
    {
        private readonly ILogger<ApiIntegrationTestFixture> _logger;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private string _databaseName;
        private static List<ErrorEntity> _errorEntities;

        public WebApplicationFactory<Program> ApiFactory { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }
        public ClientPool ClientPool { get; private set; }
        public TestUtils TestUtils { get; private set; }

        static ApiIntegrationTestFixture()
        {
            LoadLaunchSettingEnvironmentVariables();
            ProjectInitializer.Start();
        }

        public ApiIntegrationTestFixture()
        {
            var sw = Stopwatch.StartNew();
            InitializeApiFactory();
            InitializeServiceProvider();
            _logger = ServiceProvider.GetRequiredService<ILogger<ApiIntegrationTestFixture>>();
            _fireplaceApiContext = ServiceProvider.GetRequiredService<FireplaceApiContext>();
            StartDatabase();
            TestUtils = new TestUtils(this);
            ClientPool = new ClientPool(this);

            _logger.LogAppInformation($"TestFixture with database [{_databaseName}] initialized successfully.", sw);
        }

        private void InitializeApiFactory()
        {
            ApiFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        RemoveMainDatabaseAndAddTestDatabase(services);
                    });
                    builder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    }).UseNLog();
                });
        }

        private void InitializeServiceProvider()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var sp = ApiFactory.Services;
                var scope = sp.CreateScope();
                ServiceProvider = scope.ServiceProvider;
            }
            catch (Exception ex)
            {
                _logger.LogAppError($"Error: {ex.Message}", sw, ex: ex);
            }
        }

        private void StartDatabase()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _fireplaceApiContext.Database.EnsureDeleted();
                _fireplaceApiContext.Database.EnsureCreated();
                InitTestDatabase();
                _logger.LogAppInformation($"Database initialized successfully.", sw);
            }
            catch (Exception ex)
            {
                _logger.LogAppError($"An error occurred seeding the " +
                    $"database with test messages. Error: {ex.Message}", sw, ex: ex);
            }
        }

        public void CleanDatabase()
        {
            var sw = Stopwatch.StartNew();
            _fireplaceApiContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""UserEntities"" CASCADE;");
            _logger.LogAppTrace("Database cleaned successfully!", sw);
        }

        private void RemoveMainDatabaseAndAddTestDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<FireplaceApiContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var newRandomConnectionString = GenerateRandomConnectionString();
            services.AddDbContext<FireplaceApiContext>(
                optionsBuilder => optionsBuilder.UseNpgsql(newRandomConnectionString)
            );
        }

        private string GenerateRandomConnectionString()
        {
            _databaseName = Core.Tools.Utils.GenerateRandomString(8);
            var databaseNameRegex = @"^(.*)Database=([^;]+);(.*)$";
            var newConnectionString = Regex.Replace(ProjectInitializer.DatabaseConnectionString,
                databaseNameRegex, $"$1Database={_databaseName};$3", RegexOptions.IgnoreCase);
            return newConnectionString;
        }

        private void InitTestDatabase()
        {
            LoadErrors();
            _fireplaceApiContext.ErrorEntities.AddRange(_errorEntities);

            _fireplaceApiContext.SaveChanges();
            _fireplaceApiContext.DetachAllEntries();
        }

        private void LoadErrors()
        {
            if (_errorEntities != null)
                return;
            var mainFireplaceApiContext = new FireplaceApiContext(ProjectInitializer.DatabaseConnectionString);
            _errorEntities = mainFireplaceApiContext.ErrorEntities.AsNoTracking().ToList();
        }

        private static void LoadLaunchSettingEnvironmentVariables()
        {
            using var file = File.OpenText("Properties\\launchSettings.json");
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject
                .GetValue("profiles")
                .SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();

            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
            }
        }

        public void Dispose()
        {
            _fireplaceApiContext.Database.EnsureDeleted();
        }
    }
}

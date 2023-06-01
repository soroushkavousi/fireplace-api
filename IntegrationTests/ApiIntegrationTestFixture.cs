using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.IntegrationTests.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FireplaceApi.IntegrationTests;

public class ApiIntegrationTestFixture : IDisposable
{
    private string _databaseName;
    private string _databaseConnectionString;
    private readonly ProjectDbContext _dbContext;
    private readonly static Logger _logger;
    private static List<ConfigsEntity> _configsEntities;
    private static List<ErrorEntity> _errorEntities;

    public WebApplicationFactory<Program> ApiFactory { get; private set; }
    public IServiceProvider ServiceProvider { get; private set; }
    public ClientPool ClientPool { get; private set; }

    static ApiIntegrationTestFixture()
    {
        _logger = LogManager.GetCurrentClassLogger();
        LoadLaunchSettingEnvironmentVariables();
        ProjectInitializer.Start();
        ReadDatabaseInitialData();
    }

    private static void LoadLaunchSettingEnvironmentVariables()
    {
        var launchSettingsPath = @"Properties/launchSettings.json";
        if (!File.Exists(launchSettingsPath))
            return;
        using var file = File.OpenText("Properties/launchSettings.json");
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

    private static void ReadDatabaseInitialData()
    {
        var mainDbContext = new ProjectDbContext(EnvironmentVariable.ConnectionString.Value);

        if (mainDbContext.Database.GetPendingMigrations().Any())
        {
            var serverMessage = "Database migrations are not applied!!!";
            _logger.LogAppCritical(serverMessage);
            throw new InternalServerException("Database migrations are not applied!!!");
        }

        _configsEntities ??= mainDbContext.ConfigsEntities.AsNoTracking().ToList();
        if (_configsEntities.Any() == false)
        {
            var serverMessage = "No configs are found in the database!!!";
            _logger.LogAppCritical(serverMessage);
            throw new InternalServerException("No configs are found in the database!!!");
        }

        _errorEntities ??= mainDbContext.ErrorEntities.AsNoTracking().ToList();
        if (_errorEntities.Any() == false)
        {
            var serverMessage = "No errors are found in the database!!!";
            _logger.LogAppCritical(serverMessage);
            throw new InternalServerException("No errors are found in the database!!!");
        }
    }

    public ApiIntegrationTestFixture()
    {
        var sw = Stopwatch.StartNew();
        CreateDatabaseClone();
        InitializeApiFactory();
        InitializeServiceProvider();
        _dbContext = ServiceProvider.GetRequiredService<ProjectDbContext>();
        ClientPool = new ClientPool(this);

        _logger.LogAppInformation($"TestFixture with database [{_databaseName}] initialized successfully.", sw);
    }

    private void CreateDatabaseClone()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _databaseConnectionString = GenerateRandomConnectionString();
            var newDbContext = new ProjectDbContext(_databaseConnectionString);
            newDbContext.Database.EnsureDeleted();
            newDbContext.Database.EnsureCreated();
            newDbContext.ConfigsEntities.AddRange(_configsEntities);
            newDbContext.ErrorEntities.AddRange(_errorEntities);
            newDbContext.SaveChanges();
            newDbContext.DetachAllEntries();
            _logger.LogAppInformation($"New clone of database with name [{_databaseName}] successfully created.", sw);
        }
        catch (Exception ex)
        {
            _logger.LogAppError($"Can't clone the database! Error: {ex.Message}", sw, ex: ex);
            throw;
        }
    }

    private string GenerateRandomConnectionString()
    {
        _databaseName = $"test-{Domain.Tools.Utils.GenerateRandomString(8)}";
        var databaseNameRegex = @"^(.*)Database=([^;]+);(.*)$";
        var newConnectionString = Regex.Replace(EnvironmentVariable.ConnectionString.Value,
            databaseNameRegex, $"$1Database={_databaseName};$3", RegexOptions.IgnoreCase);
        return newConnectionString;
    }

    private void InitializeApiFactory()
    {
        ApiFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    ReplaceMainDatabaseWithTestDatabase(services);
                });
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                }).UseNLog();
            });
    }

    private void ReplaceMainDatabaseWithTestDatabase(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions<ProjectDbContext>));

        if (descriptor != null)
            services.Remove(descriptor);

        services.AddDbContext<ProjectDbContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(_databaseConnectionString)
        );
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
            throw;
        }
    }

    public void CleanDatabase()
    {
        var sw = Stopwatch.StartNew();
        _dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""UserEntities"" CASCADE;");
        _dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""RequestTraceEntities"" CASCADE;");
        _logger.LogAppTrace($"Database [{_databaseName}] cleaned successfully.", sw);
    }

    public void Dispose()
    {
        var sw = Stopwatch.StartNew();
        _dbContext.Database.EnsureDeleted();
        _logger.LogAppTrace($"Database [{_databaseName}] removed successfully.", sw);
    }
}

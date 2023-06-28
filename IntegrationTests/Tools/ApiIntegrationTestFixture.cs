using FireplaceApi.Application.Emails;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.IntegrationTests.Stubs;
using FireplaceApi.Presentation;
using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FireplaceApi.IntegrationTests.Tools;

public class ApiIntegrationTestFixture : IDisposable
{
    private string _databaseName;
    private string _databaseConnectionString;
    private readonly ApiDbContext _dbContext;
    private readonly static Logger _logger;
    private static List<ConfigsEntity> _configsEntities;
    private static List<ErrorEntity> _errorEntities;

    public WebApplicationFactory<Program> ApiFactory { get; private set; }
    public IServiceProvider ServiceProvider { get; private set; }
    public ClientPool ClientPool { get; private set; }

    static ApiIntegrationTestFixture()
    {
        _logger = LogManager.GetCurrentClassLogger();
        ProjectInitializer.Start();
        ReadDatabaseInitialData();
    }

    private static void ReadDatabaseInitialData()
    {
        var mainDbContext = new ApiDbContext(EnvironmentVariable.ConnectionString.Value);

        if (mainDbContext.Database.GetPendingMigrations().Any())
        {
            var serverMessage = "Database migrations are not applied!!!";
            _logger.LogServerCritical(serverMessage);
            throw new InternalServerException("Database migrations are not applied!!!");
        }

        _configsEntities ??= mainDbContext.ConfigsEntities.AsNoTracking().ToList();
        if (_configsEntities.Any() == false)
        {
            var serverMessage = "No configs are found in the database!!!";
            _logger.LogServerCritical(serverMessage);
            throw new InternalServerException("No configs are found in the database!!!");
        }

        _errorEntities ??= mainDbContext.ErrorEntities.AsNoTracking().ToList();
        if (_errorEntities.Any() == false)
        {
            var serverMessage = "No errors are found in the database!!!";
            _logger.LogServerCritical(serverMessage);
            throw new InternalServerException("No errors are found in the database!!!");
        }
    }

    public ApiIntegrationTestFixture()
    {
        var sw = Stopwatch.StartNew();
        CreateDatabaseClone();
        InitializeApiFactory();
        InitializeServiceProvider();
        _dbContext = ServiceProvider.GetRequiredService<ApiDbContext>();
        ClientPool = new ClientPool(this);

        _logger.LogServerInformation($"TestFixture with database [{_databaseName}] initialized successfully.", sw);
    }

    private void CreateDatabaseClone()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _databaseConnectionString = GenerateRandomConnectionString();
            var newDbContext = new ApiDbContext(_databaseConnectionString);
            newDbContext.Database.EnsureDeleted();
            newDbContext.Database.EnsureCreated();
            newDbContext.ConfigsEntities.AddRange(_configsEntities);
            newDbContext.ErrorEntities.AddRange(_errorEntities);
            newDbContext.SaveChanges();
            newDbContext.DetachAllEntries();
            _logger.LogServerInformation($"New clone of database with name [{_databaseName}] successfully created.", sw);
        }
        catch (Exception ex)
        {
            _logger.LogServerError($"Can't clone the database! Error: {ex.Message}", sw, ex: ex);
            throw;
        }
    }

    private string GenerateRandomConnectionString()
    {
        _databaseName = $"test-{Application.Common.Utils.GenerateRandomString(8)}";
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
                    ReplaceEmailGatewayWithStub(services);
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
            d.ServiceType == typeof(DbContextOptions<ApiDbContext>));

        if (descriptor != null)
            services.Remove(descriptor);

        services.AddDbContext<ApiDbContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(_databaseConnectionString)
        );
    }

    private void ReplaceEmailGatewayWithStub(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d =>
            d.ServiceType == typeof(IEmailGateway));

        if (descriptor != null)
            services.Remove(descriptor);

        services.AddSingleton<IEmailGateway, EmailGatewayStub>();
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
            _logger.LogServerError($"Error: {ex.Message}", sw, ex: ex);
            throw;
        }
    }

    public void CleanDatabase()
    {
        var sw = Stopwatch.StartNew();
        _dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""UserEntities"" CASCADE;");
        _dbContext.Database.ExecuteSqlRaw(@"TRUNCATE TABLE public.""RequestTraceEntities"" CASCADE;");
        _logger.LogServerTrace($"Database [{_databaseName}] cleaned successfully.", sw);
    }

    public void Dispose()
    {
        var sw = Stopwatch.StartNew();
        _dbContext.Database.EnsureDeleted();
        _logger.LogServerTrace($"Database [{_databaseName}] removed successfully.", sw);
    }
}

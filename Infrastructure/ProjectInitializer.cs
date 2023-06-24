using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;

namespace FireplaceApi.Infrastructure;

public static class ProjectInitializer
{
    private static Logger _logger;

    public static void Initialize(string logConfigFilePath, string logRootDirectoryPath,
        string dbConnectionString, string environmentName)
    {
        Logging.Initializer.Initialize(logConfigFilePath, logRootDirectoryPath);
        _logger = LogManager.GetCurrentClassLogger();
        CheckDbConnectionString(dbConnectionString);
        LoadConfigsFromTheDatabase(dbConnectionString, environmentName);
        _logger.LogAppInformation($"Project '{nameof(Infrastructure)}' has initialized successfully.");
    }

    private static void CheckDbConnectionString(string dbConnectionString)
    {
        if (string.IsNullOrWhiteSpace(dbConnectionString))
        {
            var message = "Please provide connection string!";
            _logger.LogAppCritical(message: message);
            Console.WriteLine($"Error: {message}");
            throw new Exception(message);
        }
    }

    private static void LoadConfigsFromTheDatabase(string dbConnectionString, string environmentName)
    {
        var dbContext = new ProjectDbContext(dbConnectionString);

        try
        {
            ConfigsEntity.Current = dbContext.ConfigsEntities
                .AsNoTracking()
                .SingleOrDefault(e => e.EnvironmentName == environmentName);
        }
        catch (Exception ex)
        {
            var serverMessage = "Can not get the configs from the database!!!";
            _logger.LogAppCritical(serverMessage, parameters: new { environmentName }, ex: ex);
            Configs.Current = Configs.Default;
            return;
        }

        if (ConfigsEntity.Current == null)
        {
            var serverMessage = "No configs are found in the database!!!";
            _logger.LogAppCritical(serverMessage, parameters: new { environmentName });
            Configs.Current = Configs.Default;
            return;
        }

        Configs.Current = new Configs(ConfigsEntity.Current.Id,
            ConfigsEntity.Current.EnvironmentName.ToEnum<EnvironmentName>(),
            api: ConfigsEntity.Current.Data.Api,
            file: ConfigsEntity.Current.Data.File,
            queryResult: ConfigsEntity.Current.Data.QueryResult,
            email: ConfigsEntity.Current.Data.Email,
            google: ConfigsEntity.Current.Data.Google,
            ConfigsEntity.Current.CreationDate, ConfigsEntity.Current.ModifiedDate);
    }
}

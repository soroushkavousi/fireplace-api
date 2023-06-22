using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;
using System.Linq;

namespace FireplaceApi.Presentation.Tools;

public static class ProjectInitializer
{
    private static bool _initialized = false;
    private static IConfigurationRoot _appSettings;

    public static Logger Logger { get; private set; }

    public static void Start()
    {
        if (_initialized)
            return;

        _initialized = true;
        ReadAppSettings();
        SetupLogger();
        CheckConnectionString();
        LoadConfigsFromTheDatabase();
    }

    private static void ReadAppSettings()
    {
        _appSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{EnvironmentVariable.EnvironmentName.Value}.json", optional: false, reloadOnChange: false)
            .Build();
    }

    private static void SetupLogger()
    {
        try
        {
            var logConfigFilePath = _appSettings["Log:ConfigFilePath"];
            NLogBuilder.ConfigureNLog(logConfigFilePath);
            LogManager.Configuration.Variables["logRootDirectoryPath"] = EnvironmentVariable.LogDirectory.Value;
            Logger = LogManager.GetCurrentClassLogger();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't setup logger: {ex}");
            throw;
        }
    }

    private static void CheckConnectionString()
    {
        if (!EnvironmentVariable.ConnectionString.IsProvided)
        {
            var message = "Can't find the connection string!\n";
            message += $"Please add {EnvironmentVariable.ConnectionString.Key} to the environment variables.";
            Logger.LogAppCritical(message: message);
            Console.WriteLine($"Error: {message}");
            throw new Exception(message);
        }
    }

    private static void LoadConfigsFromTheDatabase()
    {
        var dbContext = new ProjectDbContext(EnvironmentVariable.ConnectionString.Value);
        var environmentName = EnvironmentVariable.EnvironmentName.Value;

        try
        {
            ConfigsEntity.Current = dbContext.ConfigsEntities
                .AsNoTracking()
                .SingleOrDefault(e => e.EnvironmentName == environmentName);
        }
        catch (Exception ex)
        {
            var serverMessage = "Can not get the configs from the database!!!";
            Logger.LogAppCritical(serverMessage, parameters: new { environmentName }, ex: ex);
            Configs.Current = Configs.Default;
            return;
        }

        if (ConfigsEntity.Current == null)
        {
            var serverMessage = "No configs are found in the database!!!";
            Logger.LogAppCritical(serverMessage, parameters: new { environmentName });
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

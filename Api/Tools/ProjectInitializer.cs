using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Operators;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;
using System.Linq;

namespace FireplaceApi.Api.Tools
{
    public static class ProjectInitializer
    {
        private static bool _initialized = false;
        private static string _environmentName;
        private static IConfigurationRoot _config;

        public static Logger Logger { get; private set; }

        private static void Initialize()
        {
            _initialized = true;
            _environmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentNameKey);
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static void Start()
        {
            if (!_initialized)
                Initialize();

            SetupGlobalValues();
            SetupLogger();
        }

        private static void SetupGlobalValues()
        {
            var connectionString = _config.GetConnectionString(Constants.MainDatabaseKey);
            using var fireplaceApiContext = new FireplaceApiContext(connectionString);
            var environmentName = _environmentName.ToEnum<EnvironmentName>();
            GlobalOperator.GlobalValues = fireplaceApiContext.GlobalEntities
                .AsNoTracking().Where(e => e.EnvironmentName == environmentName.ToString()).Single().Values;
        }

        private static void SetupLogger()
        {
            try
            {
                NLogBuilder.ConfigureNLog(GlobalOperator.GlobalValues.Log.ConfigFilePath);
                LogManager.Configuration.Variables["logRootDirectoryPath"] =
                    GlobalOperator.GlobalValues.Log.RootDirectoryPath;
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't setup logger: {ex}");
            }
        }

        private static string ReadFromConfig(string key)
        {
            return _config.GetSection(key).Value;
        }
    }
}

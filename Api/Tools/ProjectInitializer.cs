using FireplaceApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;

namespace FireplaceApi.Api.Tools
{
    public static class ProjectInitializer
    {
        private static bool _initialized = false;
        private static IConfigurationRoot _appSettings;

        public static Logger Logger { get; private set; }
        public static string EnvironmentName { get; private set; }
        public static string DatabaseConnectionString { get; private set; }

        public static void Start()
        {
            if (_initialized)
                return;

            _initialized = true;
            ReadAppSettings();
            SetupLogger();
            ReadDatabaseConnectionString();
        }

        private static void ReadAppSettings()
        {
            EnvironmentName = Utils.GetEnvironmentVariable(Constants.EnvironmentNameKey);
            _appSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: false, reloadOnChange: false)
                .Build();
        }

        private static void SetupLogger()
        {
            try
            {
                var logConfigFilePath = _appSettings["Log:ConfigFilePath"];
                var logRootDirectoryPath = _appSettings["Log:RootDirectoryPath"];
                NLogBuilder.ConfigureNLog(logConfigFilePath);
                LogManager.Configuration.Variables["logRootDirectoryPath"] = logRootDirectoryPath;
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't setup logger: {ex}");
                throw;
            }
        }

        private static void ReadDatabaseConnectionString()
        {
            var connectionStringEnvironmentKey = _appSettings["ConnectionStringEnvironmentKey"];
            DatabaseConnectionString = Utils.GetEnvironmentVariable(connectionStringEnvironmentKey);
            if (string.IsNullOrWhiteSpace(DatabaseConnectionString))
            {
                var message = "Can't find the connection string!\n";
                message += $"Please add {connectionStringEnvironmentKey} to the environment variables.";
                Logger.LogAppError(message: message, parameters: new { DatabaseConnectionString });
                throw new Exception(message);
            }
            else
            {
                var message = $"Connection String with key {connectionStringEnvironmentKey} successfully found in environment variables. ";
                Logger.LogAppInformation(message, parameters: new { ConnectionString = DatabaseConnectionString[..10] + "..." });
            }
        }
    }
}

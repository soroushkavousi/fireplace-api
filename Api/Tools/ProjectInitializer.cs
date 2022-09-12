using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.ValueObjects;
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
            var environmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentNameKey);
            _appSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: false)
                .Build();

            Configs.Instance = _appSettings.Get<Configs>();
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
            DatabaseConnectionString = Environment.GetEnvironmentVariable(Constants.ConnectionStringKey, EnvironmentVariableTarget.Process);

            if (string.IsNullOrWhiteSpace(DatabaseConnectionString))
                DatabaseConnectionString = Environment.GetEnvironmentVariable(Constants.ConnectionStringKey, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(DatabaseConnectionString))
                DatabaseConnectionString = Environment.GetEnvironmentVariable(Constants.ConnectionStringKey, EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(DatabaseConnectionString))
            {
                var message = "Can't find the connection string!\n";
                message += $"Please add {Constants.ConnectionStringKey} to the environment variables.";
                Logger.LogAppError(message: message, parameters: new { DatabaseConnectionString });
                throw new Exception(message);
            }
        }
    }
}

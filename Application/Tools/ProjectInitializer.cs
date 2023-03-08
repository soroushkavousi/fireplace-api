using FireplaceApi.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;

namespace FireplaceApi.Application.Tools
{
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
            ReadEnvironmentVariables();
            ReadAppSettings();
            SetupLogger();
            CheckConnectionString();
        }

        private static void ReadEnvironmentVariables()
        {
            EnvironmentVariable.EnvironmentName.ReadValue();
            EnvironmentVariable.LogDirectory.ReadValue();
            EnvironmentVariable.ConnectionString.ReadValue();
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
    }
}

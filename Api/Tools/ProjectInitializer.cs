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

        public static void Start()
        {
            if (_initialized)
                return;

            _initialized = true;
            ReadAppSettings();
            SetupLogger();
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
            }
        }
    }
}

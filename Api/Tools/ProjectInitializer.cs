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
        private static string _environmentName;
        private static IConfigurationRoot _configurationBuilder;

        public static Logger Logger { get; private set; }

        private static void Initialize()
        {
            _initialized = true;
            _environmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentNameKey);
            _configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: false, reloadOnChange: true)
                .Build();
            Configs.Instance = _configurationBuilder.Get<Configs>();
        }

        public static void Start()
        {
            if (!_initialized)
                Initialize();

            SetupLogger();
        }

        private static void SetupLogger()
        {
            try
            {
                NLogBuilder.ConfigureNLog(Configs.Instance.Log.ConfigFilePath);
                LogManager.Configuration.Variables["logRootDirectoryPath"] =
                    Configs.Instance.Log.RootDirectoryPath;
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't setup logger: {ex}");
            }
        }
    }
}

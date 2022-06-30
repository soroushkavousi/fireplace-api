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

        public static Logger Logger { get; private set; }

        public static void Start()
        {
            if (_initialized)
                return;
            
            _initialized = true;
            InitializeConfigs();
            SetupLogger();
        }

        private static void InitializeConfigs()
        {
            var environmentName = Environment.GetEnvironmentVariable(Constants.EnvironmentNameKey);
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: false)
                .Build();
            Configs.Instance = configurationBuilder.Get<Configs>();
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

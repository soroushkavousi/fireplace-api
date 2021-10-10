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
        private static IConfigurationRoot _config;

        public static Logger Logger { get; private set; }

        public static void Start()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            SetupGlobalValues();
            SetupLogger();
        }

        public static void SetupGlobalValues()
        {
            var globalId = ReadFromConfig("GlobalId").ToEnum<GlobalId>();
            var connectionString = _config.GetConnectionString("MainDatabase");
            var fireplaceApiContext = new FireplaceApiContext(connectionString);
            GlobalOperator.GlobalValues = fireplaceApiContext.GlobalEntities
                .AsNoTracking().Where(e => e.Id == globalId.To<int>()).Single().Values;
        }

        public static void SetupLogger()
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

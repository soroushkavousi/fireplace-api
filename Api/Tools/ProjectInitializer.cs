using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GamingCommunityApi.Api.Tools
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
            var gamingCommunityApiContext = new GamingCommunityApiContext(connectionString);
            GlobalOperator.GlobalValues = gamingCommunityApiContext.GlobalEntities
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
                Console.WriteLine($"Can't setup logger: {ex.ToString()}");
            }
        }

        private static string ReadFromConfig(string key)
        {
            return _config.GetSection(key).Value;
        }
    }
}

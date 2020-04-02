using GamingCommunityApi.Core.Tools;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GamingCommunityApi.Api.Tools.NLog
{
    public static class NLogManager
    {
        public static LogFactory LogFactory { get; set; }

        public static void ConfigureNLog()
        {
            var relativeLogConfigPath = @"Codes\Api\Tools\NLog\nlog.config";
            var relativeLogRootFolder = @"Logs";
            var solutionDirectory = Core.Tools.Utils.GetSolutionDirectory();
            var logConfigPath = Path.Combine(solutionDirectory, relativeLogConfigPath);
            var logRootFolder = Path.Combine(solutionDirectory, relativeLogRootFolder);

            //Utils.CreateParentDirectoriesOfFileIfNotExists(logRootFolder);
            NLogBuilder.ConfigureNLog(logConfigPath);
            LogManager.Configuration.Variables["logRootFolder"] = logRootFolder;
        }

        public static Logger GetLogger()
        {
            return LogManager.GetCurrentClassLogger();
        }
    }
}

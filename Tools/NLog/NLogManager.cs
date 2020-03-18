using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GamingCommunityApi.Tools.NLog
{
    public static class NLogManager
    {
        public static LogFactory LogFactory { get; set; }

        public static void ConfigureNLog()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string logRootFolder;
            if (isWindows)
                logRootFolder = @"C:\Bitiano\GamingCommunity\GamingCommunityApi\Logs";
            else
                logRootFolder = @"/home/bitianist/Bitiano/GamingCommunity/GamingCommunityApi/Logs";

            //Utils.CreateParentDirectoriesOfFileIfNotExists(logRootFolder);
            NLogBuilder.ConfigureNLog(@"Tools/NLog/nlog.config");
            LogManager.Configuration.Variables["logRootFolder"] = logRootFolder;
        }

        public static Logger GetLogger()
        {
            return LogManager.GetCurrentClassLogger();
        }
    }
}

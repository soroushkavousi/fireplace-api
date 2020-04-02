using GamingCommunityApi.Api.Tools.NLog;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Api.Tools
{
    public static class Utils
    {
        public static Logger SetupLogger()
        {
            Logger logger = null;
            try
            {
                NLogManager.ConfigureNLog();
                logger = NLogManager.GetLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't setup logger: {ex.ToString()}");
            }
            return logger;
        }

    }
}

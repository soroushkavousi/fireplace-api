using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public static class Utils
    {
        public static bool IsOsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
        public static DateTimeOffset GetYesterdayDate() 
            => DateTimeOffset.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0));
        public static DateTimeOffset GetLastHourDate()
            => DateTimeOffset.UtcNow.Subtract(new TimeSpan(0, 1, 0, 0));
    }
}

using System;
using System.Runtime.InteropServices;

namespace FireplaceApi.Api.Tools
{
    public static class Utils
    {
        public static string ContentRootPath
        {
            get
            {
                var rootDirectory = AppContext.BaseDirectory;
                if (rootDirectory.Contains("bin"))
                {
                    rootDirectory = rootDirectory[..rootDirectory.IndexOf("bin")];
                }
                return rootDirectory;
            }
        }

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

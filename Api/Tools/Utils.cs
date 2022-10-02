using System;
using System.Runtime.InteropServices;

namespace FireplaceApi.Api.Tools
{
    public static class Utils
    {
        public static string GetEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

            if (string.IsNullOrWhiteSpace(value))
                value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(value))
                value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

            return value;
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

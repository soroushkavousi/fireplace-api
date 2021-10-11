using Microsoft.Extensions.Logging;
using NLog;
using System.Diagnostics;

namespace FireplaceApi.Core.Extensions
{
    public static class LogExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static long Finish(this Stopwatch sw)
        {
            if (sw == null)
                return 0;
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void LogTrace<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogTrace(logMessage, executionTime);
        }

        public static void LogIOTrace<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.LogTrace(logMessage, executionTime);
        }

        public static void LogInformation<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogInformation(logMessage, executionTime);
        }

        public static void LogIOInformation<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.LogInformation(logMessage, executionTime);
        }

        public static void LogTrace(this Logger logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.Trace(logMessage, executionTime);
        }

        public static void LogIOTrace(this Logger logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.Trace(logMessage, executionTime);
        }

        public static void LogInformation(this Logger logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.Info(logMessage, executionTime);
        }

        public static void LogIOInformation(this Logger logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.Info(logMessage, executionTime);
        }

        public static string CreateLogMessage(string section, object parameters)
        {
            var message = $"#{section} | Parameters: {parameters.ToJson()}";
            return CreateLogMessage(message);
        }

        public static string CreateLogMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                message = "None";
            else
                message = message.EscapeCurlyBrackets();

            var logMessage = $"{{executionTime}}ms | {message}".Trim();
            return logMessage;
        }
    }
}

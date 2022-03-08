using FireplaceApi.Core.Tools;
using Microsoft.Extensions.Logging;
using NLog;
using System;
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

        public static void LogAppTrace<T>(this ILogger<T> logger, string message = "")
            => LogAppTrace(logger, null, message);
        public static void LogAppTrace<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogTrace(logMessage, executionTime);
        }

        public static void LogAppIOTrace<T>(this ILogger<T> logger, string section = "", object parameters = null)
            => LogAppIOTrace(logger, null, section, parameters);
        public static void LogAppIOTrace<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.LogTrace(logMessage, executionTime);
        }

        public static void LogAppInformation<T>(this ILogger<T> logger, string message = "")
            => LogAppInformation(logger, null, message);
        public static void LogAppInformation<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogInformation(logMessage, executionTime);
        }

        public static void LogAppIOInformation<T>(this ILogger<T> logger, string section = "", object parameters = null)
            => LogAppIOInformation<T>(logger, null, section, parameters);
        public static void LogAppIOInformation<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.LogInformation(logMessage, executionTime);
        }

        public static void LogAppError<T>(this ILogger<T> logger, string message = "")
            => LogAppError(logger, default(Stopwatch), message);
        public static void LogAppError<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogError(logMessage, executionTime);
        }

        public static void LogAppError<T>(this ILogger<T> logger, Exception ex = null, string message = "")
            => LogAppError<T>(logger, null, ex, message);
        public static void LogAppError<T>(this ILogger<T> logger, Stopwatch sw, Exception ex = null, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogError(ex, logMessage, executionTime);
        }

        public static void LogAppCritical<T>(this ILogger<T> logger, Exception ex = null, string message = "")
            => LogAppCritical<T>(logger, null, ex, message);
        public static void LogAppCritical<T>(this ILogger<T> logger, Stopwatch sw, Exception ex = null, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.LogCritical(ex, logMessage, executionTime);
        }

        public static void LogAppTrace(this Logger logger, string message = "")
            => LogAppTrace(logger, null, message);
        public static void LogAppTrace(this Logger logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.Trace(logMessage, executionTime);
        }

        public static void LogAppIOTrace(this Logger logger, string section = "", object parameters = null)
        => LogAppIOTrace(logger, null, parameters);
        public static void LogAppIOTrace(this Logger logger, Stopwatch sw, string section = "", object parameters = null)
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(section, parameters);
            logger.Trace(logMessage, executionTime);
        }

        public static void LogAppInformation(this Logger logger, string message = "")
            => LogAppInformation(logger, null, message);
        public static void LogAppInformation(this Logger logger, Stopwatch sw, string message = "")
        {
            var executionTime = sw.Finish();
            var logMessage = CreateLogMessage(message);
            logger.Info(logMessage, executionTime);
        }

        public static void LogAppIOInformation(this Logger logger, string section = "", object parameters = null)
            => LogAppIOInformation(logger, null, section, parameters);
        public static void LogAppIOInformation(this Logger logger, Stopwatch sw, string section = "", object parameters = null)
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

            message = Regexes.SensitiveInformation.Replace(message, "$1$2$3$4$5$7***$8");
            var logMessage = $"{{executionTime}}ms | {message}".Trim();
            return logMessage;
        }
    }
}

using Microsoft.Extensions.Logging;
using NLog;
using System.Diagnostics;

namespace FireplaceApi.Core.Extensions
{
    public static class LogExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void LogTrace<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            sw.Stop();
            var logMessage = CreateLogMessage(message);
            logger.LogTrace(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogIOTrace<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object inputs = null, object outputs = null)
        {
            sw.Stop();
            var logMessage = CreateLogMessage(section, inputs, outputs);
            logger.LogTrace(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogInformation<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            sw.Stop();
            var logMessage = CreateLogMessage(message);
            logger.LogInformation(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogIOInformation<T>(this ILogger<T> logger, Stopwatch sw, string section = "", object inputs = null, object outputs = null)
        {
            sw.Stop();
            var logMessage = CreateLogMessage(section, inputs, outputs);
            logger.LogInformation(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogTrace(this Logger logger, Stopwatch sw, string message = "")
        {
            sw.Stop();
            var logMessage = CreateLogMessage(message);
            logger.Trace(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogIOTrace(this Logger logger, Stopwatch sw, string section = "", object inputs = null, object outputs = null)
        {
            sw.Stop();
            var logMessage = CreateLogMessage(section, inputs, outputs);
            logger.Trace(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogInformation(this Logger logger, Stopwatch sw, string message = "")
        {
            sw.Stop();
            var logMessage = CreateLogMessage(message);
            logger.Info(logMessage, sw.ElapsedMilliseconds);
        }

        public static void LogIOInformation(this Logger logger, Stopwatch sw, string section = "", object inputs = null, object outputs = null)
        {
            sw.Stop();
            var logMessage = CreateLogMessage(section, inputs, outputs);
            logger.Info(logMessage, sw.ElapsedMilliseconds);
        }

        public static string CreateLogMessage(string section, object inputs, object outputs)
        {
            var message = $"#{section} | Inputs: {inputs.ToJson()} | Outputs: {outputs.ToJson()}";
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
